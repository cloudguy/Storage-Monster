using CloudBin.Web.UI.Resources;
using System.Globalization;
using System.Web.Mvc;

namespace CloudBin.Web.UI.Services
{
    public static class PageExtensions
    {
        public static void SetTitleAndDescription(this WebViewPage page, string title, string description)
        {
            page.ViewBag.Title = string.Format(CultureInfo.CurrentCulture, "{0} - {1}", ApplicationResources.AppName, title);
            page.ViewBag.Description = description;
        }

        public static string GetTitle(this WebViewPage page)
        {
            string title = page.ViewBag.Title as string;
            return string.IsNullOrWhiteSpace(title) ? ApplicationResources.AppName : title;
        }

        public static string GetDescription(this WebViewPage page)
        {
            string description = page.ViewBag.Description as string;
            return string.IsNullOrWhiteSpace(description) ? ApplicationResources.AppName : description;
        }
    }
}