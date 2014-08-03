using System.Collections.Generic;

namespace CloudBin.Core.Configuration
{
    public interface IGlobalizationConfiguration
    {
        IEnumerable<Locale> Locales { get; }
        Locale DefaultLocale { get; }
    }
}
