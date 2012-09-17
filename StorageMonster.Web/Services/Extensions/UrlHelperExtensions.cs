using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Extensions
{
    public static class UrlHelperExtensions
    {
        public static bool IsLocalUrl(this UrlHelper helper, string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            Uri absoluteUri;
            if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
            {
                return String.Equals(helper.RequestContext.HttpContext.Request.Url.Host, absoluteUri.Host,
                    StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                bool isLocal = !url.StartsWith("http:", StringComparison.OrdinalIgnoreCase)
                    && !url.StartsWith("https:", StringComparison.OrdinalIgnoreCase)
                    && Uri.IsWellFormedUriString(url, UriKind.Relative);
                return isLocal;
            }
        }
    }
}
