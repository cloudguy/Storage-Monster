using CloudBin.Core;
using CloudBin.Web.Core.Html;
using System;
using System.Globalization;
using System.Web.Mvc;

namespace CloudBin.Web.UI.Services
{
    public static class HtmlExtensions
    {
        private static readonly Lazy<ILocaleProvider> LocaleProviderLazy = new Lazy<ILocaleProvider>(() =>
        {
            return (ILocaleProvider)DependencyResolver.Current.GetService(typeof(ILocaleProvider));
        });

        public static MvcHtmlString CurrentLocaleFlag(this CloudBinHtmlHelper helper)
        {
            return LocaleFlag(helper, LocaleProviderLazy.Value.GetThreadLocale());
        }

        public static MvcHtmlString LocaleFlag(this CloudBinHtmlHelper helper, Locale locale)
        {
            UrlHelper urlHelper = new UrlHelper(helper.HtmlHelper.ViewContext.RequestContext);
            TagBuilder imgBuilder = new TagBuilder("img");
            imgBuilder.Attributes.Add("src", urlHelper.Content("~/Content/images/blank.gif"));
            imgBuilder.Attributes.Add("alt", locale.FullName);
            imgBuilder.AddCssClass("flag");
            RegionInfo regionInfo = new RegionInfo(locale.Culture.LCID);
            imgBuilder.AddCssClass(string.Format(CultureInfo.InvariantCulture, "flag-{0}", regionInfo.TwoLetterISORegionName.ToLowerInvariant()));
            return new MvcHtmlString(imgBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}