using System;
using System.Globalization;
using System.Text;
using System.Web;

namespace CloudBin.Web.Utilities
{
    public static class CookieHelper
    {
        public static string GetCookieName(string cookieName, string appRoot)
        {
            if (string.IsNullOrEmpty(appRoot))
                return cookieName;
            return string.Format(CultureInfo.InvariantCulture, "{0}_{1}", cookieName, Base64EncodeForCookieName(appRoot));
        }

        public static string GetCookieName(string cookieName)
        {
            return GetCookieName(cookieName, HttpContext.Current.Request.ApplicationPath);
        }

        private static string Base64EncodeForCookieName(string s)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s)).Replace('+', '.').Replace('/', '-').Replace('=', '_');
        }
    }
}
