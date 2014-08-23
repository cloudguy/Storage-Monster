using System.IO;
using System.Text;

namespace CloudBin.Web.Core.Optimization
{
    public interface IMarkupMinificationFilterFactory
    {
        Stream CreateHtmlMinifierFilter(Stream responseFilter, string rawUrl, Encoding encoding);
    }
}
