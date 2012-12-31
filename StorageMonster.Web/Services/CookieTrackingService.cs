using System;
using System.Collections.Generic;
using System.Configuration;
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
        public CookieTrackingService(ILocaleProvider localeProvider)
        {
            _localeProvider = localeProvider;
        }

        public void SetLocaleTracking(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (!context.Response.IsClientConnected)
                return;
            var localeData = RequestContext.GetValue<LocaleData>(_localeProvider.LocaleKey);
#warning don't set cookie every time
            if (localeData != null)
            {
                WebConfigurationSection configuration = (WebConfigurationSection)ConfigurationManager.GetSection(WebConfigurationSection.SectionLocation);
                string cookieName = CookieHelper.GetCookieName(configuration.Tracking.CookieName, HttpContext.Current.Request.ApplicationPath);
                HttpCookie localeCookie = new HttpCookie(cookieName, localeData.ShortName);
                localeCookie.Expires = DateTime.UtcNow.AddMinutes(configuration.Tracking.CookieExpiration);
                context.Response.SetCookie(localeCookie);
            }
        }

        public string GetTrackedLocaleName(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            WebConfigurationSection configuration = (WebConfigurationSection)ConfigurationManager.GetSection(WebConfigurationSection.SectionLocation);
            string cookieName = CookieHelper.GetCookieName(configuration.Tracking.CookieName, HttpContext.Current.Request.ApplicationPath);
            var cookie = context.Request.Cookies.Get(cookieName);
            if (cookie != null)
                return cookie.Value;
            return null;
        }
    }
}