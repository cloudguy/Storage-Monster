using System;
using System.Globalization;
using System.IO;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.ActionResults
{
    public class BufferLessFileResult: ActionResult
    {
        private readonly Stream _stream;
        private readonly string _fileName;
        private const int BufferSize = 4096;

        public BufferLessFileResult(Stream stream, string fileName)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            _stream = stream;

            _fileName = string.IsNullOrEmpty(fileName) ? "file" : fileName;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.BufferOutput = false;
            response.Buffer = false;

            response.ContentType = Constants.BinaryStreamContentType;

            response.AddHeader("Content-Disposition", string.Format(CultureInfo.InvariantCulture, "attachment; filename=\"{0}\"", _fileName));
            if (_stream.CanSeek)
                response.AddHeader("Content-Length", _stream.Length.ToString(CultureInfo.InvariantCulture));
            
            using (response.OutputStream)
            {
                byte[] buffer = new byte[BufferSize];
                int count;
                while ((count = _stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    response.OutputStream.Write(buffer, 0, count);
                    response.Flush();
                }
            }
        }
    }
}