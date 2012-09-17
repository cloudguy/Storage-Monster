using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Globalization;

namespace StorageMonster.Web.Services.Upload
{
    public class FileUploader
    {       
        public class RequestItems
        {
            public IDictionary<string, string> Parameters { get; set; }            
        }

        private HttpWorkerRequest _workerRequest;
        private Encoding _encoding;
        private byte[] _multipartBoundary;

        public FileUploader(HttpWorkerRequest workerRequest)
        {
            _workerRequest = workerRequest;
        }

        public bool ProccessRequest(IServiceProvider provider, string rootPath)
        {
            if (_workerRequest == null)
                throw new InvalidOperationException("WorkerRequest was not set");

            if (!_workerRequest.HasEntityBody())            
                return false;
            
            _encoding = MultipartFormHelper.GetEncodingFromHeaders(_workerRequest) ?? Encoding.UTF8;
            _multipartBoundary = MultipartFormHelper.GetMultipartBoundary(_workerRequest);

            if (_multipartBoundary == null)
                return false;

            var total = _workerRequest.GetTotalEntityBodyLength();
            var preloaded = _workerRequest.GetPreloadedEntityBodyLength();
            var loaded = preloaded;

                        var buffer = new byte[1024];  

            var st = RequestBufferlessStream.CreateStream(_workerRequest);
            var psst = MultipartParserStream.CreateStream(st, _encoding, _multipartBoundary);

            int read;
            int totalRead = 0;
            string str = "";

            psst.ParseFileNameAndContentType();
            using (var fs = new FileStream(rootPath + psst.Filename, FileMode.Create, FileAccess.Write))
            {
                while ((read = psst.Read(buffer, 0, buffer.Length)) > 0)
                {
                    //str = str+ _encoding.GetString(buffer, 0, read);
                    fs.Write(buffer, 0, read);
                    fs.Flush();
                    totalRead += read;
                }
            }         

            return true;
        }
    }
}