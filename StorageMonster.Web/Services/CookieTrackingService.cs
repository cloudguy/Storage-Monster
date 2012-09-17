using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StorageMonster.Services;
using StorageMonster.Utilities;
using StorageMonster.Web.Services.Configuration;

namespace StorageMonster.Web.Services
{
    public class CookieTrackingService : ITrackingService
    {
        private readonly ILocaleProvider _localeProvider;
        private readonly IWebConfiguration _webConfiguration;
        public CookieTrackingService(ILocaleProvider localeProvider, IWebConfiguration webConfiguration)
        {
            _localeProvider = localeProvider;
            _webConfiguration = webConfiguration;
        }

        public void SetLocaleTracking(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            var localeData = RequestContext.GetValue<LocaleData>(RequestContext.LocaleKey);
#warning don't set cookie every time
            if (localeData != null)
            {
                HttpCookie localeCookie = new HttpCookie(_webConfiguration.LocaleCookieName, RequestContext.GetValue<LocaleData>(RequestContext.LocaleKey).ShortName);
                localeCookie.Expires = DateTime.UtcNow.Add(_webConfiguration.LocaleCookieTimeout);
                context.Response.SetCookie(localeCookie);
            }
        }

        public string GetTrackedLocaleName(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var cookie = context.Request.Cookies.Get(_webConfiguration.LocaleCookieName);
            if (cookie != null)
                return cookie.Value;
            return null;
        }
    }
}