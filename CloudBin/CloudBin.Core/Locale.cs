using System;
using System.Globalization;

namespace CloudBin.Core
{
    [Serializable]
    public class Locale
    {
        public Locale(string name, string fullName, CultureInfo culture, CultureInfo uiCulture)
        {
            Name = name;
            FullName = fullName;
            Culture = culture;
            UiCulture = uiCulture;
        }
        public CultureInfo Culture { get; private set; }
        public CultureInfo UiCulture { get; private set; }
        public String FullName { get; private set; }
        public String Name { get; private set; }
    }
}
