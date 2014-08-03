using System.Collections.Generic;
using CloudBin.Core.Configuration;

namespace CloudBin.Core
{
    public interface ILocaleProvider
    {
        IEnumerable<Locale> SupportedLocales { get; }
        Locale DefaultLocale { get; }
        Locale GetLocaleByNameOrDefault(string name);
        void SetThreadLocale(Locale locale);
        void Initialize(IGlobalizationConfiguration globalizationConfiguration);
    }
}
