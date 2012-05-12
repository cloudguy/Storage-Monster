using System;
using System.Collections.Generic;
using System.Linq;

namespace StorageMonster.Services
{
    internal class LocaleProvider : ILocaleProvider
    {
        protected static List<LocaleData> SupportedLocalesInternal = new List<LocaleData>();
        protected static LocaleData DefaultLocale;
        public LocaleData DefaultCulture { get { return DefaultLocale; } }

        public IEnumerable<LocaleData> SupportedLocales
        {
            get { return SupportedLocalesInternal; }
        }

        public void Init(LocaleData[] supportedLocales, LocaleData defaultLocale)
        {
            SupportedLocalesInternal.AddRange(supportedLocales);
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
    }
}
