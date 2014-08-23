using System.Collections.Generic;
using System.Configuration;

namespace CloudBin.Web.Core.Configuration
{
    public sealed class ThemingXmlConfiguration : IThemingConfiguration
    {
        private static IEnumerable<ThemeDescription> _themes;
        private static readonly object Locker = new object();
        private static bool _initialized;

        IEnumerable<ThemeDescription> IThemingConfiguration.Themes
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

                return _themes;
            }
        }

        private void InitializeThemes()
        {
            ThemingConfigurationSection configSection = (ThemingConfigurationSection) ConfigurationManager.GetSection(ThemingConfigurationSection.SectionLocation);
            List<ThemeDescription> themes = new List<ThemeDescription>();
            ThemeDescription defaultTheme = null;

            foreach (ThemeConfigurationElement element in configSection.Locales)
            {
                ThemeDescription currentTheme = new ThemeDescription(element.Name, element.IsDefault);
                if (element.IsDefault)
                {
                    if (defaultTheme != null)
                    {
                        throw new ConfigurationErrorsException("More than one default locale specified");
                    }
                    defaultTheme = currentTheme;
                }
                themes.Add(currentTheme);
            }

            if (defaultTheme == null)
            {
                throw new ConfigurationErrorsException("Default theme not specified");
            }
            _themes = themes;
            _initialized = true;
        }
    }
}
