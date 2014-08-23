using System.Web.Mvc;

namespace CloudBin.Web.Core.Html
{
    public static class HtmlHelperExtensions
    {
        public static CloudBinHtmlHelper CloudBin(this HtmlHelper htmlHelper)
        {
            return new CloudBinHtmlHelper(htmlHelper);
        }
    }
}
