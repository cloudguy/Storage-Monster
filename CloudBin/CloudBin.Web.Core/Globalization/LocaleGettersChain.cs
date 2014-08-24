using CloudBin.Core;
using CloudBin.Web.Core.Security;
using CloudBin.Web.Core.Tracking;
using System;
using System.Web;

namespace CloudBin.Web.Core.Globalization
{
    internal static class LocaleGettersChain
    {
        private static readonly Lazy<ILocaleProvider> LocaleProviderLazy = new Lazy<ILocaleProvider>(() =>
        {
            return (ILocaleProvider)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(ILocaleProvider));
        });

        private static readonly Lazy<ITrackingService> TrackingServiceLazy = new Lazy<ITrackingService>(() =>
        {
            return (ITrackingService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(ITrackingService));
        });

        public static Locale GetLocale(HttpApplication application)
        {
            Func<HttpApplication, Locale>[] localeGetters =
            {
                TryGetLocaleFromIdentity,
                TryGetLocaleFromTrackingService,
                TryGetLocaleFromuserLanguages
            };
            Locale locale = null;
            foreach (Func<HttpApplication, Locale> localeGetter in localeGetters)
            {
                locale = localeGetter(application);
                if (locale != null)
                {
                    break;
                }
            }

            return locale ?? LocaleProviderLazy.Value.DefaultLocale;
        }

        private static Locale TryGetLocaleFromIdentity(HttpApplication application)
        {
            Identity identity = application.Context.User.Identity as Identity;
            if (identity != null && identity.IsAuthenticated)
            {
                return LocaleProviderLazy.Value.GetLocaleByNameOrDefault(identity.Locale);
            }
            return null;
        }

        private static Locale TryGetLocaleFromTrackingService(HttpApplication application)
        {
            Locale locale = null;
            string localeName;
            if (TrackingServiceLazy.Value.TryGetTrackedValue(Constants.LocaleTrackingKey, out localeName))
            {
                LocaleProviderLazy.Value.TryFindLocaleByName(localeName, out locale);
            }
            return locale;
        }

        private static Locale TryGetLocaleFromuserLanguages(HttpApplication application)
        {
            HttpRequest request = application.Request;
            Locale locale = null;
            if (request.UserLanguages != null && request.UserLanguages.Length != 0)
            {
                LocaleProviderLazy.Value.TryFindLocaleByName(request.UserLanguages[0], out locale);
            }
            return locale;
        }
    }
}
