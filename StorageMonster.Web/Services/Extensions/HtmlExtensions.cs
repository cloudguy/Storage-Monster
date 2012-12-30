using System.Collections.Generic;
using System.Web.Routing;
using StorageMonster.Web.Services.Configuration;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString IncludeJs(this HtmlHelper html, string[] urls)
        {
            var sb = new StringBuilder();

            var config = (WebConfigurationSection)ConfigurationManager.GetSection(WebConfigurationSection.SectionLocation);
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            foreach (string url in urls)
            {
                var builder = new TagBuilder("script");
                builder.Attributes.Add("type", "text/javascript");
                builder.Attributes.Add("src", string.Format(CultureInfo.InvariantCulture, "{0}?{1}", urlHelper.Content(url), config.ClientObjects.JsVersion));
                sb.AppendLine(builder.ToString(TagRenderMode.Normal));
            }
            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString IncludeCss(this HtmlHelper html, params string[] urls)
        {
            var sb = new StringBuilder();
            var config = (WebConfigurationSection)ConfigurationManager.GetSection(WebConfigurationSection.SectionLocation);
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            foreach (string url in urls)
            {
                var builder = new TagBuilder("link");
                builder.Attributes.Add("rel", "stylesheet");
                builder.Attributes.Add("type", "text/css");
                builder.Attributes.Add("href", string.Format(CultureInfo.InvariantCulture, "{0}?{1}", urlHelper.Content(url), config.ClientObjects.CssVersion));
                sb.AppendLine(builder.ToString(TagRenderMode.SelfClosing));
            }
            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString RequestSuccessInfo(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            IEnumerable<string> messages = htmlHelper.ViewData.GetRequestSuccessMessages();
            if (messages == null)
                return null;

            StringBuilder htmlBuilder = new StringBuilder();

            foreach (var message in messages)
            {
                if (string.IsNullOrEmpty(message))
                    continue;
                TagBuilder divBuilder = new TagBuilder("div");
                divBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
                divBuilder.AddCssClass("request-info-summary");

                TagBuilder spanBuilder = new TagBuilder("span");
                spanBuilder.SetInnerText(message);
                divBuilder.InnerHtml = spanBuilder.ToString(TagRenderMode.Normal);
                htmlBuilder.Append(divBuilder.ToString(TagRenderMode.Normal));
            }

            return MvcHtmlString.Create(htmlBuilder.ToString());
        }

        public static MvcHtmlString RequestSuccessInfo(this HtmlHelper htmlHelper)
        {
            return RequestSuccessInfo(htmlHelper, null);
        }


        public static MvcHtmlString RequestErrorInfo(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            IEnumerable<string> messages = htmlHelper.ViewData.GetRequestErrorMessages();
            if (messages == null)
                return null;

            StringBuilder htmlBuilder = new StringBuilder();

            foreach (var message in messages)
            {
                if (string.IsNullOrEmpty(message))
                    continue;
                TagBuilder divBuilder = new TagBuilder("div");
                divBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
                divBuilder.AddCssClass("request-error-summary");

                TagBuilder spanBuilder = new TagBuilder("span");
                spanBuilder.SetInnerText(message);
                divBuilder.InnerHtml = spanBuilder.ToString(TagRenderMode.Normal);
                htmlBuilder.Append(divBuilder.ToString(TagRenderMode.Normal));
            }

            return MvcHtmlString.Create(htmlBuilder.ToString());
        }

        public static MvcHtmlString RequestErrorInfo(this HtmlHelper htmlHelper)
        {
            return RequestErrorInfo(htmlHelper, null);
        }
    }
}