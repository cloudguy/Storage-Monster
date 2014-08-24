using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudBin.Core;
using CloudBin.Web.Core.Html;

namespace CloudBin.Web.UI.Services
{
    public static class HtmlExtensions
    {
        private static readonly Lazy<ILocaleProvider> LocaleProviderLazy = new Lazy<ILocaleProvider>(() =>
        {
            return (ILocaleProvider)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(ILocaleProvider));
        });

        public static MvcHtmlString LocaleFlag(this CloudBinHtmlHelper helper, Locale locale)
        {
            UrlHelper urlHelper = new UrlHelper(helper.HtmlHelper.ViewContext.RequestContext);
            TagBuilder imgBuilder = new TagBuilder("img");
            imgBuilder.Attributes.Add("src", urlHelper.Content("~/Content/images/blank.gif"));
            imgBuilder.Attributes.Add("alt", locale.FullName);
            imgBuilder.AddCssClass("flag");
            imgBuilder.AddCssClass(string.Format(CultureInfo.InvariantCulture,"flag-{0}", locale.Culture.TwoLetterISOLanguageName));
            return new MvcHtmlString(imgBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}