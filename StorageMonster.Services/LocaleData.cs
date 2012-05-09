using System;
using System.Globalization;

namespace StorageMonster.Services
{
    public class LocaleData
    {
        public CultureInfo Culture { get; set; }
        public String FullName { get; set; }
        public String ShortName { get; set; }
    }
}
