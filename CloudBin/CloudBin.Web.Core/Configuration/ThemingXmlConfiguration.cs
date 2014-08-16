using System.Collections.Generic;
using System.Configuration;

namespace CloudBin.Web.Core.Configuration
{
    public sealed class ThemingXmlConfiguration : IThemingConfiguration
    {
        private static IEnumerable<string> _themesNames;
        private static string _defaultThemeName;
        private static readonly object Locker = new object();
        private static bool _initialized;

        IEnumerable<string> IThemingConfiguration.ThemeNames
        {
            get
            {
                if (!_initialized)
                {
                    lock (Locker)
                    {
                        if (!_initialized)
                        {
                            InitializeThemes();
                        }
                    }
                }

                return _themesNames;
            }
        }

        string IThemingConfiguration.DefaultThemeName
        {
            get
            {
                if (!_initialized)
                {
                    lock (Locker)
                    {
                        if (!_initialized)
                        {
                            InitializeThemes();
                        }
                    }
                }

                return _defaultThemeName;
            }
        }

        private void InitializeThemes()
        {
            ThemingConfigurationSection configSection = (ThemingConfigurationSection) ConfigurationManager.GetSection(ThemingConfigurationSection.SectionLocation);
            List<string> themeNames = new List<string>();
            string defaultThemeName = null;

            foreach (ThemeConfigurationElement element in configSection.Locales)
            {
                themeNames.Add(element.Name);
                if (element.IsDefault)
                {
                    if (defaultThemeName != null)
                    {
                        throw new ConfigurationErrorsException("More than one default locale specified");
                    }
                    defaultThemeName = element.Name;
                }
            }

            if (defaultThemeName == null)
            {
                throw new ConfigurationErrorsException("Default theme not specified");
            }

            _defaultThemeName = defaultThemeName;
            _themesNames = themeNames;
            _initialized = true;
        }
    }
}
