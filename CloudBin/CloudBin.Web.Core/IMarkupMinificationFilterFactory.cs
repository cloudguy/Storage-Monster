using System.IO;
using System.Text;

namespace CloudBin.Web.Core
{
    public interface IMarkupMinificationFilterFactory
    {
        Stream CreateHtmlMinifierFilter(Stream responseFilter, string rawUrl, Encoding encoding);
    }
}
