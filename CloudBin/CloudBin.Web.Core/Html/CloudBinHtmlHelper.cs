using System.Web.Mvc;

namespace CloudBin.Web.Core.Html
{
    public sealed class CloudBinHtmlHelper
    {
        private readonly HtmlHelper _htmlHelper;

        internal CloudBinHtmlHelper(HtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        internal HtmlHelper HtmlHelper
        {
            get { return _htmlHelper; }
        }
    }
}
