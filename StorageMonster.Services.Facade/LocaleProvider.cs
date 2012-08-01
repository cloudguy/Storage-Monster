using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Globalization;
using StorageMonster.Utilities;

namespace StorageMonster.Services.Facade
{
    public class LocaleProvider : ILocaleProvider
    {
        protected static ConcurrentBag<LocaleData> SupportedLocalesInternal = new ConcurrentBag<LocaleData>();
        protected static LocaleData DefaultLocale;
        public LocaleData DefaultCulture { get { return DefaultLocale; } }

        public IEnumerable<LocaleData> SupportedLocales
        {
            get { return SupportedLocalesInternal; }
        }

        public void Init(LocaleData[] supportedLocales, LocaleData defaultLocale)
        {
            foreach (var supportedLocale in supportedLocales)
                SupportedLocalesInternal.Add(supportedLocale);
            DefaultLocale = defaultLocale;
        }

        public LocaleData GetCultureByName(string name)
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
            RequestContext.SetValue(RequestContext.LocaleKey, locale);
			Thread.CurrentThread.CurrentUICulture = locale.Culture;
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(locale.Culture.Name);
        }
    }
}