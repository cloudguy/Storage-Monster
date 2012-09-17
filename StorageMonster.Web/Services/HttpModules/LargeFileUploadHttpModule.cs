using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using StorageMonster.Web.Services.Upload;
using Common.Logging;

namespace StorageMonster.Web.Services.HttpModules
{
    

    internal class StaticWorkerRequest : HttpWorkerRequest
    {
        readonly HttpWorkerRequest _request;
        private readonly byte[] _buffer;

        public StaticWorkerRequest(HttpWorkerRequest request, byte[] buffer)
        {
            _request = request;
            _buffer = buffer;
        }

        public override int ReadEntityBody(byte[] buffer, int size)
        {
            return 0;
        }

        public override int ReadEntityBody(byte[] buffer, int offset, int size)
        {
            return 0;
        }

        public override byte[] GetPreloadedEntityBody()
        {
            return _buffer;
        }

        public override int GetPreloadedEntityBody(byte[] buffer, int offset)
        {
            Buffer.BlockCopy(_buffer, 0, buffer, offset, _buffer.Length);
            return _buffer.Length;
        }

        public override int GetPreloadedEntityBodyLength()
        {
            return _buffer.Length;
        }

        public override int GetTotalEntityBodyLength()
        {
            return _buffer.Length;
        }

        public override string GetKnownRequestHeader(int index)
        {
            return index == HeaderContentLength
                       ? "0"
                       : _request.GetKnownRequestHeader(index);
        }

        #region Passthrough

        public override string GetUriPath()
        {
            return _request.GetUriPath();
        }

        public override string GetQueryString()
        {
            return _request.GetQueryString();
        }

        public override string GetRawUrl()
        {
            return _request.GetRawUrl();
        }

        public override string GetHttpVerbName()
        {
            return _request.GetHttpVerbName();
        }

        public override string GetHttpVersion()
        {
            return _request.GetHttpVersion();
        }

        public override string GetRemoteAddress()
        {
            return _request.GetRemoteAddress();
        }

        public override int GetRemotePort()
        {
            return _request.GetRemotePort();
        }

        public override string GetLocalAddress()
        {
            return _request.GetLocalAddress();
        }

        public override int GetLocalPort()
        {
            return _request.GetLocalPort();
        }

        public override void SendStatus(int statusCode, string statusDescription)
        {
            _request.SendStatus(statusCode, statusDescription);
        }

        public override void SendKnownResponseHeader(int index, string value)
        {
            _request.SendKnownResponseHeader(index, value);
        }

        public override void SendUnknownResponseHeader(string name, string value)
        {
            _request.SendUnknownResponseHeader(name, value);
        }

        public override void SendResponseFromMemory(byte[] data, int length)
        {
            _request.SendResponseFromMemory(data, length);
        }

        public override void SendResponseFromFile(string filename, long offset, long length)
        {
            _request.SendResponseFromFile(filename, offset, length);
        }

        public override void SendResponseFromFile(IntPtr handle, long offset, long length)
        {
            _request.SendResponseFromFile(handle, offset, length);
        }

        public override bool IsEntireEntityBodyIsPreloaded()
        {
            return true;
        }

        public override void FlushResponse(bool finalFlush)
        {
            _request.FlushResponse(finalFlush);
        }

        public override void EndOfRequest()
        {
            _request.EndOfRequest();
        }

        public override void CloseConnection()
        {
            _request.CloseConnection();
        }
        #endregion
    }

    internal class UploadProcessor
    {
        private byte[] _buffer;
        private byte[] _boundaryBytes;
        private byte[] _endHeaderBytes;
        private byte[] _endFileBytes;
        private byte[] _lineBreakBytes;

        private const string _lineBreak = "\r\n";

        private readonly Regex _filename =
            new Regex(@"Content-Disposition:\s*form-data\s*;\s*name\s*=\s*""(.+)""\s*;\s*filename\s*=\s*""(.*)""",
                      RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly HttpWorkerRequest _workerRequest;


        public string SavedFileName
        {
            get;
            set;
        }

        public UploadProcessor(HttpWorkerRequest workerRequest)
        {
            _workerRequest = workerRequest;
        }

        public void StreamToDisk(IServiceProvider provider, Encoding encoding, string rootPath)
        {
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            //this._encoding = this.GetEncodingFromHeaders();
		//if (this._encoding == null)

            var buffer = new byte[8192];

            if (!_workerRequest.HasEntityBody())
            {
                return;
            }

            var total = _workerRequest.GetTotalEntityBodyLength();
            var preloaded = _workerRequest.GetPreloadedEntityBodyLength();
            var loaded = preloaded;

            SetByteMarkers(_workerRequest, encoding);

            var body = _workerRequest.GetPreloadedEntityBody();
            if (body == null) // IE normally does not preload
            {
                body = new byte[8192];
                preloaded = _workerRequest.ReadEntityBody(body, body.Length);
                loaded = preloaded;
            }

            var text = encoding.GetString(body);
            var matchcollection = _filename.Matches(text);
            var fileName = matchcollection[0].Groups[2].Value;
            fileName = Path.GetFileName(fileName); // IE captures full user path; chop it
            
            SavedFileName = FileCheckRename(rootPath, fileName);
            var path = Path.Combine(rootPath, SavedFileName);

            var files = new List<string> { fileName };
            var stream = new FileStream(path, FileMode.Create);

            if (preloaded > 0)
            {
                stream = ProcessHeaders(body, stream, encoding, preloaded, files, rootPath);
            }

            // Used to force further processing (i.e. redirects) to avoid buffering the files again
            var workerRequest = new StaticWorkerRequest(_workerRequest, body);
            var field = HttpContext.Current.Request.GetType().GetField("_wr", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(HttpContext.Current.Request, workerRequest);

            if (!_workerRequest.IsEntireEntityBodyIsPreloaded())
            {
                var received = preloaded;
                while (total - received >= loaded && _workerRequest.IsClientConnected())
                {
                    loaded = _workerRequest.ReadEntityBody(buffer, buffer.Length);
                    stream = ProcessHeaders(buffer, stream, encoding, loaded, files, rootPath);

                    received += loaded;
                }

                var remaining = (total == preloaded) ? preloaded : total - received;
                buffer = new byte[remaining];

                loaded = _workerRequest.ReadEntityBody(buffer, remaining);
                stream = ProcessHeaders(buffer, stream, encoding, loaded, files, rootPath);
            }

            stream.Flush();
            stream.Close();
            stream.Dispose();
        }

        // 기존 파일 여부 확인 및 파일명 리네임.
        private string FileCheckRename(string rootPath, string fileName)
        {
            string forwardName = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            int i = 1;
            List<FileDescription> savedFiles = GetUploadedFiles(rootPath);
            while (savedFiles.Exists(f => f.Name == fileName))
            {
                fileName = string.Format("{0}({1}){2}", forwardName, i++, extension);
            }
            return fileName;
        }

        private void SetByteMarkers(HttpWorkerRequest workerRequest, Encoding encoding)
        {
            var contentType = workerRequest.GetKnownRequestHeader(HttpWorkerRequest.HeaderContentType);
            var bufferIndex = contentType.IndexOf("boundary=") + "boundary=".Length;
            var boundary = String.Concat("--", contentType.Substring(bufferIndex));

            _boundaryBytes = encoding.GetBytes(string.Concat(boundary, _lineBreak));
            _endHeaderBytes = encoding.GetBytes(string.Concat(_lineBreak, _lineBreak));
            _endFileBytes = encoding.GetBytes(string.Concat(_lineBreak, boundary, "--", _lineBreak));
            _lineBreakBytes = encoding.GetBytes(string.Concat(_lineBreak + boundary + _lineBreak));
        }

        private FileStream ProcessHeaders(byte[] buffer, FileStream stream, Encoding encoding, int count, ICollection<string> files, string rootPath)
        {       


            buffer = AppendBuffer(buffer, count);

            var startIndex = IndexOf(buffer, _boundaryBytes, 0);
            if (startIndex != -1)
            {
                var endFileIndex = IndexOf(buffer, _endFileBytes, 0);
                if (endFileIndex != -1)
                {
                    var precedingBreakIndex = IndexOf(buffer, _lineBreakBytes, 0);
                    if (precedingBreakIndex > -1)
                    {
                        startIndex = precedingBreakIndex;
                    }

                    endFileIndex += _endFileBytes.Length;

                    var modified = SkipInput(buffer, startIndex, endFileIndex, ref count);
                    stream.Write(modified, 0, count);
                }
                else
                {
                    var endHeaderIndex = IndexOf(buffer, _endHeaderBytes, 0);
                    if (endHeaderIndex != -1)
                    {
                        endHeaderIndex += _endHeaderBytes.Length;

                        var text = encoding.GetString(buffer);
                        var match = _filename.Match(text);

                        var fileName = match != null ? match.Groups[2].Value : null;
                        fileName = Path.GetFileName(fileName); // IE captures full user path; chop it

                        if (!string.IsNullOrEmpty(fileName) && !files.Contains(fileName))
                        {
                            files.Add(fileName);

                            var filePath = Path.Combine(rootPath, fileName);

                            stream = ProcessNextFile(stream, buffer, count, startIndex, endHeaderIndex, filePath);
                        }
                        else
                        {
                            var modified = SkipInput(buffer, startIndex, endHeaderIndex, ref count);
                            stream.Write(modified, 0, count);
                        }
                    }
                    else
                    {
                        _buffer = buffer;
                    }
                }
            }
            else
            {
                stream.Write(buffer, 0, count);
            }

            return stream;
        }

        private static FileStream ProcessNextFile(FileStream stream, byte[] buffer, int count, int startIndex, int endIndex, string filePath)
        {
            var fullCount = count;
            var endOfFile = SkipInput(buffer, startIndex, count, ref count);
            stream.Write(endOfFile, 0, count);

            stream.Flush();
            stream.Close();
            stream.Dispose();

            stream = new FileStream(filePath, FileMode.Create);

            var startOfFile = SkipInput(buffer, 0, endIndex, ref fullCount);
            stream.Write(startOfFile, 0, fullCount);

            return stream;
        }

        private static int IndexOf(byte[] array, IList<byte> value, int startIndex)
        {
            var index = 0;
            var start = Array.IndexOf(array, value[0], startIndex);

            if (start == -1)
            {
                return -1;
            }

            while ((start + index) < array.Length)
            {
                if (array[start + index] == value[index])
                {
                    index++;
                    if (index == value.Count)
                    {
                        return start;
                    }
                }
                else
                {
                    start = Array.IndexOf(array, value[0], start + index);

                    if (start != -1)
                    {
                        index = 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }

            return -1;
        }

        private static byte[] SkipInput(byte[] input, int startIndex, int endIndex, ref int count)
        {
            var range = endIndex - startIndex;
            var size = count - range;

            var modified = new byte[size];
            var modifiedCount = 0;

            for (var i = 0; i < input.Length; i++)
            {
                if (i >= startIndex && i < endIndex)
                {
                    continue;
                }

                if (modifiedCount >= size)
                {
                    break;
                }

                modified[modifiedCount] = input[i];
                modifiedCount++;
            }

            input = modified;
            count = modified.Length;
            return input;
        }

        private byte[] AppendBuffer(byte[] buffer, int count)
        {
            var input = new byte[_buffer == null ? buffer.Length : _buffer.Length + count];
            if (_buffer != null)
            {
                Buffer.BlockCopy(_buffer, 0, input, 0, _buffer.Length);
            }
            Buffer.BlockCopy(buffer, 0, input, _buffer == null ? 0 : _buffer.Length, count);
            _buffer = null;

            return input;
        }



        /// <summary>
        /// 업로드 폴더의 모든 파일정보를 가져온다.
        /// </summary>
        /// <returns></returns>
        public List<FileDescription> GetUploadedFiles(string uploadFolder)
        {
            List<FileDescription> fileDescriptions = new List<FileDescription>();
            string[] files = Directory.GetFiles(uploadFolder);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                fileDescriptions.Add(
                    new FileDescription
                    {
                        Name = fileInfo.Name,
                        Size = fileInfo.Length,
                        CreatedDate = fileInfo.CreationTime
                    });
            }
            return fileDescriptions;
        }
    }


    /// <summary>
    /// 파일정보 클래스.
    /// </summary>
    public class FileDescription
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class LargeFileUploadHttpModule : IHttpHandler 
    {      
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            app_BeginRequest(context);
        }

       
        void app_BeginRequest(HttpContext context)
        {
#warning check max length
            //HttpContext context = ((HttpApplication)sender).Context;

           // if (context.Request.ContentLength > 4096)
            //{
                /*IServiceProvider provider = (IServiceProvider)context;
                HttpWorkerRequest wr = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));

                UploadProcessor p = new UploadProcessor(wr);
               
                p.StreamToDisk(provider, Encoding.UTF8, context.Server.MapPath("~/Uploads/"));*/
            
                IServiceProvider provider = (IServiceProvider)context;
                HttpWorkerRequest wr = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));

                FileUploader f = new FileUploader(wr);
                f.ProccessRequest(provider, context.Server.MapPath("~/Uploads/"));
                byte[] data = Encoding.UTF8.GetBytes("ok");
                wr.SendStatus(200, "OK OK OK");
                wr.SendResponseFromMemory(data, data.Length);
                wr.FlushResponse(true);
                wr.EndOfRequest();
           

            //context.Response.Write("Ok");
            //context.Response.End();



                /* IServiceProvider provider = (IServiceProvider)context;
                HttpWorkerRequest wr = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));

                int ContentLength;
                byte[] Buffer;
                int Received = 0;
                int TotalReceived = 0;
                int DefaultBufferLength = 1024;
                ContentLength = wr.GetTotalEntityBodyLength();




                using (var ms = new MemoryStream())
                {
                    Buffer = wr.GetPreloadedEntityBody();
                    Received = Buffer.Length;
                    TotalReceived += Received;
                    ms.Write(Buffer, 0, Buffer.Length);

                    if (!wr.IsEntireEntityBodyIsPreloaded())
                    {
                        while ((ContentLength - TotalReceived) >= DefaultBufferLength)
                        {
                            Buffer = new Byte[DefaultBufferLength];
                            Received = wr.ReadEntityBody(Buffer, DefaultBufferLength);
                            TotalReceived += Received;
                            ms.Write(Buffer, 0, Buffer.Length);
                            ms.Flush();
                        }


                        Buffer = new Byte[DefaultBufferLength];
                        Received = wr.ReadEntityBody(Buffer, (ContentLength - TotalReceived));
                        TotalReceived += Received;
                        ms.Write(Buffer, 0, (ContentLength - TotalReceived));
                        ms.Flush();
                    }
                    ms.Seek(0, SeekOrigin.Begin);
                    MultipartParser parser = new MultipartParser(ms);
                    if (parser.Success)
                    {
                        using (var fs = new FileStream(context.Server.MapPath("~/Uploads/" + "testfile_ps.txt"), FileMode.Truncate))
                        {
                            fs.Write(parser.FileContents, 0, parser.FileContents.Length);
                            fs.Flush();
                        }
                    }
                }*/
                
                

                //  context.Application("Result") = "";
                //    context.Response.Redirect("Upload_Error.aspx?size=" & ContentLength)
                //context.Response.Write("Ok");
                //context.Response.End();
            //}
            
            

           /*
           IServiceProvider provider = (IServiceProvider)context;
                HttpWorkerRequest wr = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));

                int ContentLength;
                byte[] Buffer;
                int Received = 0;
                int TotalReceived = 0;
                int DefaultBufferLength = 1024;
                ContentLength = wr.GetTotalEntityBodyLength();


                

                using (var fs = new FileStream(context.Server.MapPath("~/Uploads/" + "testfile.txt"), FileMode.Truncate))
                {
                    Buffer = wr.GetPreloadedEntityBody();
                    Received = Buffer.Length;
                    TotalReceived += Received;
                    fs.Write(Buffer, 0, Buffer.Length);

                    if (!wr.IsEntireEntityBodyIsPreloaded())
                    {
                        while ((ContentLength - TotalReceived) >= DefaultBufferLength)
                        {
                            Buffer = new Byte[DefaultBufferLength];
                            Received = wr.ReadEntityBody(Buffer, DefaultBufferLength);
                            TotalReceived += Received;
                            fs.Write(Buffer, 0, Buffer.Length);
                            fs.Flush();
                        }


                        Buffer = new Byte[DefaultBufferLength];
                        Received = wr.ReadEntityBody(Buffer, (ContentLength - TotalReceived));
                        TotalReceived += Received;
                        fs.Write(Buffer, 0, (ContentLength - TotalReceived));
                        fs.Flush();
                    }


                }
                //  context.Application("Result") = "";
                //    context.Response.Redirect("Upload_Error.aspx?size=" & ContentLength)
                context.Response.Write("Ok");
                context.Response.End();
            }*/

            

             

              /*  IServiceProvider provider = (IServiceProvider)context;
                HttpWorkerRequest wr = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));
                FileStream fs = null;
                // Check if body contains data
                if (wr.HasEntityBody())
                {
                    // get the total body length
                    int requestLength = wr.GetTotalEntityBodyLength();
                    // Get the initial bytes loaded
                    int initialBytes = wr.GetPreloadedEntityBody().Length;
         
                    if (!wr.IsEntireEntityBodyIsPreloaded())
                    {
                        byte[] buffer = new byte[512000];
                        //string[] fileName = context.Request.QueryString["fileName"].Split(new char[] { '\\' });
                        //fs = new FileStream(context.Server.MapPath("~/Uploads/" + fileName[fileName.Length-1]), FileMode.CreateNew);
                        fs = new FileStream(context.Server.MapPath("~/Uploads/" + "testfile"), FileMode.OpenOrCreate);
                        // Set the received bytes to initial bytes before start reading
                        int receivedBytes = initialBytes;
                        while (requestLength - receivedBytes >= initialBytes)
                        {
                            // Read another set of bytes
                            initialBytes = wr.ReadEntityBody(buffer, buffer.Length);
                            // Write the chunks to the physical file
                            fs.Write(buffer, 0, buffer.Length);
                            // Update the received bytes
                            receivedBytes += initialBytes;
                            fs.Flush();
                        }
                        initialBytes = wr.ReadEntityBody(buffer, requestLength - receivedBytes);
                       
                    }
                }
                 fs.Flush();
                 fs.Close();
                // context.Response.Redirect("~/");
                context.Response.Write("zzzz");
            }*/

        }
        public void Dispose()
        {
        }

        
    }
}