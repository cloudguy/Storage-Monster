using System.Security.Cryptography.X509Certificates;
using CloudBin.Web.Core.Theming;
using System.Collections.Generic;

namespace CloudBin.Web.Core.Configuration
{
    public interface IThemingConfiguration
    {
        IEnumerable<ThemeDescription> Themes { get; }
    }
}
