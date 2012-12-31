using System;
using System.Globalization;
using System.Text;

namespace StorageMonster.Web.Services
{
    public static class CookieHelper
    {
        public static string GetCookieName(string cookieName, string appRoot)
        {
            if (string.IsNullOrEmpty(appRoot))
                return cookieName;
            return string.Format(CultureInfo.InvariantCulture, "{0}_{1}", cookieName, Base64EncodeForCookieName(appRoot));
        }

        private static string Base64EncodeForCookieName(string s)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s)).Replace('+', '.').Replace('/', '-').Replace('=', '_');
        }
    }
}