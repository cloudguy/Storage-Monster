using System.IO;
using System.Text;
using WebMarkupMin.Core.Minifiers;

namespace CloudBin.Web.WebMarkupMin
{
    public abstract class MarkupMinificationFilterStreamBase<T> : Stream where T : IMarkupMinifier
    {
        /// <summary>
        /// Original stream
        /// </summary>
        private readonly Stream _stream;

        /// <summary>
        /// Stream that original content is read into
        /// and then passed to TransformStream function
        /// </summary>
        private readonly MemoryStream _cacheStream = new MemoryStream();

        /// <summary>
        /// Markup minifier
        /// </summary>
        private readonly T _minifier;

        /// <summary>
        /// Current URL
        /// </summary>
        private readonly string _currentUrl;

        /// <summary>
        /// Text encoding
        /// </summary>
        private readonly Encoding _encoding;

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return 0; }
        }

        public override long Position
        {
            get;
            set;
        }


        /// <summary>
        /// Constructs instance of markup minification response filter
        /// </summary>
        /// <param name="stream">Content stream</param>
        /// <param name="minifier">Markup minifier</param>
        protected MarkupMinificationFilterStreamBase(Stream stream, T minifier)
            : this(stream, minifier, string.Empty, Encoding.Default)
        { }

        /// <summary>
        /// Constructs instance of HTML minification response filter
        /// </summary>
        /// <param name="stream">Content stream</param>
        /// <param name="minifier">Markup minifier</param>
        /// <param name="currentUrl">Current URL</param>
        /// <param name="encoding">Text encoding</param>
        protected MarkupMinificationFilterStreamBase(Stream stream, T minifier, string currentUrl, Encoding encoding)
        {
            _stream = stream;
            _minifier = minifier;
            _currentUrl = currentUrl;
            _encoding = encoding;
        }


        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _cacheStream.Write(buffer, 0, count);
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override void Close()
        {
            byte[] cacheBytes = _cacheStream.ToArray();
            int cacheSize = cacheBytes.Length;

            bool isMinified = false;

            string content = _encoding.GetString(cacheBytes);

            MarkupMinificationResult minificationResult = _minifier.Minify(content, _currentUrl, _encoding, false);
            if (minificationResult.Errors.Count == 0)
            {
                byte[] output = _encoding.GetBytes(minificationResult.MinifiedContent);
                _stream.Write(output, 0, output.GetLength(0));
                isMinified = true;
            }

            if (!isMinified)
            {
                _stream.Write(cacheBytes, 0, cacheSize);
            }

            _cacheStream.SetLength(0);
            _cacheStream.Close();
            _stream.Close();
        }
    }
}
