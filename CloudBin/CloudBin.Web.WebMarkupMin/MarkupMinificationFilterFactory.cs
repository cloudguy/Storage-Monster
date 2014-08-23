using CloudBin.Web.Core;
using System.IO;
using System.Text;
using WebMarkupMin.Core;
using WebMarkupMin.Core.Loggers;
using WebMarkupMin.Core.Minifiers;
using WebMarkupMin.Core.Settings;

namespace CloudBin.Web.WebMarkupMin
{
    public sealed class MarkupMinificationFilterFactory : IMarkupMinificationFilterFactory
    {
        Stream IMarkupMinificationFilterFactory.CreateHtmlMinifierFilter(Stream responseFilter, string rawUrl, Encoding encoding)
        {
            HtmlMinificationSettings innerSettings = WebMarkupMinContext.Current.Markup.GetHtmlMinificationSettings();
            ICssMinifier innerCssMinifier = WebMarkupMinContext.Current.Code.CreateDefaultCssMinifierInstance();
            IJsMinifier innerJsMinifier = WebMarkupMinContext.Current.Code.CreateDefaultJsMinifierInstance();
            ILogger innerLogger = WebMarkupMinContext.Current.GetDefaultLoggerInstance();
            HtmlMinifier htmlMinifier = new HtmlMinifier(innerSettings, innerCssMinifier, innerJsMinifier, innerLogger);
            return new HtmlMinificationFilterStream(responseFilter, htmlMinifier, rawUrl, encoding);
        }
    }
}
