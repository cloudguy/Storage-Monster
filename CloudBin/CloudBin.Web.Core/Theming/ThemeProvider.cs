using CloudBin.Web.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace CloudBin.Web.Core.Theming
{
    public sealed class ThemeProvider : IThemeProvider
    {
        private static Theme _defaultThemeInternal;
        private static IEnumerable<Theme> _supportedThemesInternal;
        private readonly IThemingConfiguration _themingConfiguration;

        public ThemeProvider(IThemingConfiguration themingConfiguration)
        {
            _themingConfiguration = themingConfiguration;
        }

        IEnumerable<Theme> IThemeProvider.SupportedThemes { get { return _supportedThemesInternal; } }
        Theme IThemeProvider.DefaultTheme { get { return _defaultThemeInternal; } }

        Theme IThemeProvider.GetThemeByNameOrDefault(string name)
        {
            Theme theme = _supportedThemesInternal.FirstOrDefault(l => string.Compare(l.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
            return theme ?? _defaultThemeInternal;
        }

        void IThemeProvider.Initialize(ResourceManager resourceManager)
        {
            _supportedThemesInternal = _themingConfiguration.ThemeNames.Select(themeName => new Theme(themeName, resourceManager)).ToArray();
            _defaultThemeInternal = new Theme(_themingConfiguration.DefaultThemeName, resourceManager);
        }
    }
}
