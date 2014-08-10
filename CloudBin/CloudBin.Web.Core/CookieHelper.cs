using System;
using System.Globalization;
using System.Text;
using System.Web;

namespace CloudBin.Web.Core
{
    public static class CookieHelper
    {
        public static string GetCookieName(string cookieName, string appRoot)
        {
            if (string.IsNullOrEmpty(appRoot))
            {
                return cookieName;
            }
            return string.Format(CultureInfo.InvariantCulture, "{0}_{1}", cookieName, Base64EncodeForCookieName(appRoot));
        }

        public static string GetCookieName(string cookieName)
        {
            return GetCookieName(cookieName, HttpContext.Current.Request);
        }

        public static string GetCookieName(string cookieName, HttpRequest request)
        {
            return GetCookieName(cookieName, request.ApplicationPath);
        }

        public static void ExpireCookie(HttpCookie httpCookie, HttpContext httpContext)
        {
            httpCookie.Expires = DateTime.UtcNow.AddYears(-1);
            httpContext.Response.Cookies.Add(httpCookie);
        }

        private static string Base64EncodeForCookieName(string s)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s)).Replace('+', '.').Replace('/', '-').Replace('=', '_');
        }
    }
}
