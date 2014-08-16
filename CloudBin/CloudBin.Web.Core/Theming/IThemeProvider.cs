using System.Collections.Generic;
using System.Resources;

namespace CloudBin.Web.Core.Theming
{
    public interface IThemeProvider
    {
        IEnumerable<Theme> SupportedThemes { get; }
        Theme DefaultTheme { get; }
        Theme GetThemeByNameOrDefault(string name);
        void Initialize(ResourceManager resourceManager);
    }
}
