using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace CloudBin.Core.Configuration
{
    public sealed class GlobalizationXmlConfiguration : IGlobalizationConfiguration
    {
        private static readonly object Locker = new object();
        private static bool _initialized;
        private static IEnumerable<Locale> _locales;
        private static Locale _defaultLocale;

        IEnumerable<Locale> IGlobalizationConfiguration.Locales
        {
            get
            {
                if (!_initialized)
                {
                    lock (Locker)
                    {
                        if (!_initialized)
                        {
                            InitializeLocales();
                        }
                    }
                }

                return _locales;
            }
        }

        Locale IGlobalizationConfiguration.DefaultLocale
        {
            get
            {
                if (!_initialized)
                {
                    lock (Locker)
                    {
                        if (!_initialized)
                        {
                            InitializeLocales();
                        }
                    }
                }

                return _defaultLocale;
            }
        }

        private void InitializeLocales()
        {
            GlobalizationConfigurationSection configSection = (GlobalizationConfigurationSection)ConfigurationManager.GetSection(GlobalizationConfigurationSection.SectionLocation);
            List<Locale> locales = new List<Locale>();
            Locale defaultLocale = null;

            foreach (LocaleConfigurationElement element in configSection.Locales)
            {
                var locale = new Locale(element.Name, element.FullName, CultureInfo.GetCultureInfo(element.Culture), CultureInfo.GetCultureInfo(element.UiCulture));
                locales.Add(locale);
                if (element.IsDefault)
                {
                    if (defaultLocale != null)
                    {
                        throw new ConfigurationErrorsException("More than one default locale specified");
                    }
                    defaultLocale = locale;
                }
            }

            if (defaultLocale == null)
            {
                throw new ConfigurationErrorsException("Default locale not specified");
            }

            _defaultLocale = defaultLocale;
            _locales = locales;
            _initialized = true;
        }
    }
}
