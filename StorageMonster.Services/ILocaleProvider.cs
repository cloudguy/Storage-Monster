﻿using System.Collections.Generic;

namespace StorageMonster.Services
{
    public interface ILocaleProvider
    {
        string LocaleKey { get; set; }
        IEnumerable<LocaleData> SupportedLocales { get; }
        LocaleData DefaultCulture { get; }
        void Init(LocaleData[] supportedLocales, LocaleData defaultLocale);
        LocaleData GetCultureByNameOrDefault(string name);
        void SetThreadLocale(LocaleData locale);
    }
}
