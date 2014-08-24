using CloudBin.Core.Configuration;
using CloudBin.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CloudBin.Core
{
    public sealed class LocaleProvider : ILocaleProvider
    {
        private static Locale _defaultLocaleInternal;
        private static IEnumerable<Locale> _supportedLocalesInternal;
        private const string LocaleKey = "current_request_locale";

        IEnumerable<Locale> ILocaleProvider.SupportedLocales
        {
            get { return _supportedLocalesInternal; }
        }

        Locale ILocaleProvider.DefaultLocale
        {
            get { return _defaultLocaleInternal; }
        }

        Locale ILocaleProvider.GetLocaleByNameOrDefault(string name)
        {
            Locale locale = _supportedLocalesInternal.FirstOrDefault(l => string.Compare(l.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
            return locale ?? _defaultLocaleInternal;
        }

        bool ILocaleProvider.TryFindLocaleByName(string name, out Locale locale)
        {
            locale = _supportedLocalesInternal.FirstOrDefault(l => string.Compare(l.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
            return locale != null;
        }

        void ILocaleProvider.Initialize(IGlobalizationConfiguration globalizationConfiguration)
        {
            _defaultLocaleInternal = globalizationConfiguration.DefaultLocale;
            _supportedLocalesInternal = globalizationConfiguration.Locales;
        }

        void ILocaleProvider.SetThreadLocale(Locale locale)
        {
            Verify.NotNull(()=>locale);
            RequestContext.Current.SetValue(LocaleKey, locale);
            Thread.CurrentThread.CurrentUICulture = locale.UiCulture;
            Thread.CurrentThread.CurrentCulture = locale.Culture;
        }

        Locale ILocaleProvider.GetThreadLocale()
        {
            return RequestContext.Current.GetValue<Locale>(LocaleKey);
        }
    }
}
