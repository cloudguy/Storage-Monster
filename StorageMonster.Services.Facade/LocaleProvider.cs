using StorageMonster.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace StorageMonster.Services.Facade
{
    public class LocaleProvider : ILocaleProvider
    {
        private static readonly IList<LocaleData> SupportedLocalesInternal = new List<LocaleData>();
        private static LocaleData _defaultLocale;
        private static string _localeKey = "locale_key";

        public LocaleData DefaultCulture { get { return _defaultLocale; } }

        public string LocaleKey 
        {
            get { return _localeKey; }
            set
            { 
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");
                _localeKey = value;
            }
        }

        public IEnumerable<LocaleData> SupportedLocales
        {
            get { return SupportedLocalesInternal; }
        }

        public void Init(LocaleData[] supportedLocales, LocaleData defaultLocale)
        {
            if (supportedLocales == null)
                throw new ArgumentNullException("supportedLocales");

            if (defaultLocale == null)
                throw new ArgumentNullException("defaultLocale");

            foreach (var supportedLocale in supportedLocales)
                SupportedLocalesInternal.Add(supportedLocale);
            _defaultLocale = defaultLocale;
        }

        public LocaleData GetCultureByNameOrDefault(string name)
        {
            LocaleData info = SupportedLocales.Where(l => string.Compare(l.ShortName, name, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(l => l)
                .FirstOrDefault();
            
            if (info == null)
                return DefaultCulture;
            return info;
        }
        public void SetThreadLocale(LocaleData locale)
        {
            if (locale == null)
                throw new ArgumentNullException("locale");

            RequestContext.SetValue(LocaleKey, locale);
			Thread.CurrentThread.CurrentUICulture = locale.Culture;
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(locale.Culture.Name);
        }
    }
}