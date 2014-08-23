using System.IO;
using System.Text;
using WebMarkupMin.Core.Minifiers;

namespace CloudBin.Web.WebMarkupMin
{
    public sealed class HtmlMinificationFilterStream : MarkupMinificationFilterStreamBase<HtmlMinifier>
    {
        /// <summary>
        /// Constructs instance of HTML minification response filter
        /// </summary>
        /// <param name="stream">Content stream</param>
        /// <param name="minifier">HTML minifier</param>
        public HtmlMinificationFilterStream(Stream stream, HtmlMinifier minifier)
            : base(stream, minifier)
        { }

        /// <summary>
        /// Constructs instance of HTML minification response filter
        /// </summary>
        /// <param name="stream">Content stream</param>
        /// <param name="minifier">HTML minifier</param>
        /// <param name="currentUrl">Current URL</param>
        /// <param name="encoding">Text encoding</param>
        public HtmlMinificationFilterStream(Stream stream, HtmlMinifier minifier, string currentUrl, Encoding encoding)
            : base(stream, minifier, currentUrl, encoding)
        { }
    }
}
