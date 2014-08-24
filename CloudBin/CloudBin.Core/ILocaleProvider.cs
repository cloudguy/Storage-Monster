using System.Collections.Generic;
using CloudBin.Core.Configuration;

namespace CloudBin.Core
{
    public interface ILocaleProvider
    {
        IEnumerable<Locale> SupportedLocales { get; }
        Locale DefaultLocale { get; }
        Locale GetLocaleByNameOrDefault(string name);
        bool TryFindLocaleByName(string name, out Locale locale); 
        void SetThreadLocale(Locale locale);
        Locale GetThreadLocale();
        void Initialize(IGlobalizationConfiguration globalizationConfiguration);
    }
}
