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
        private bool _initialized;

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
            if (_initialized)
            {
                return;
            }
            _supportedThemesInternal = _themingConfiguration.Themes.Select(t => new Theme(t.Name, t.IsDefault, resourceManager)).ToArray();
            _defaultThemeInternal = _supportedThemesInternal.First(t => t.IsDefault);
            _initialized = true;
        }
    }
}
