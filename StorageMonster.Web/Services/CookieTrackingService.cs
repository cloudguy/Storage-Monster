using StorageMonster.Services;
using StorageMonster.Web.Services.Configuration;
using System;
using System.Configuration;
using System.Web;

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

            var localeData = _localeProvider.GetThreadLocale(false);

            if (localeData != null)
            {
                WebConfigurationSection configuration = (WebConfigurationSection)ConfigurationManager.GetSection(WebConfigurationSection.SectionLocation);
                string cookieName = CookieHelper.GetCookieName(configuration.Tracking.CookieName, context.Request.ApplicationPath);
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
            string cookieName = CookieHelper.GetCookieName(configuration.Tracking.CookieName, context.Request.ApplicationPath);
            var cookie = context.Request.Cookies.Get(cookieName);
            if (cookie != null)
                return cookie.Value;
            return null;
        }
    }
}