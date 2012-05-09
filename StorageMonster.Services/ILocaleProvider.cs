using System.Collections.Generic;

namespace StorageMonster.Services
{
    public interface ILocaleProvider
    {
        IEnumerable<LocaleData> SupportedLocales { get; }
        LocaleData DefaultCulture { get; }
        void Init(LocaleData[] supportedLocales, LocaleData defaultLocale);
        LocaleData GetCultureByName(string name);
    }
}
