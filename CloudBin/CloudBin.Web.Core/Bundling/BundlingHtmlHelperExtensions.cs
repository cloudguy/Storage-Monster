using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudBin.Web.Core.Bundling
{
    public static class BundlingHtmlHelperExtensions
    {
        private static readonly Lazy<IBundleProvider> BundleProviderLazy = new Lazy<IBundleProvider>(() =>
        {
            return (IBundleProvider)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IBundleProvider));
        });

        public static MvcHtmlString RenderScriptBundle(this HtmlHelper helper, string bundleName)
        {
            return RenderScriptBundle(helper, bundleName, null);
        }

        public static MvcHtmlString RenderScriptBundle(this HtmlHelper helper, string bundleName, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("script");
            tagBuilder.Attributes.Add("type", "text/javascript");
            tagBuilder.Attributes.Add("src", BundleProviderLazy.Value.GetBundleUrl(bundleName).ToString());
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        public static MvcHtmlString RenderStyleBundle(this HtmlHelper helper, string bundleName)
        {
            return RenderStyleBundle(helper, bundleName, null);
        }

        public static MvcHtmlString RenderStyleBundle(this HtmlHelper helper, string bundleName, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("link");
            tagBuilder.Attributes.Add("type", "text/css");
            tagBuilder.Attributes.Add("rel", "stylesheet");
            tagBuilder.Attributes.Add("href", BundleProviderLazy.Value.GetBundleUrl(bundleName).ToString());
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}
