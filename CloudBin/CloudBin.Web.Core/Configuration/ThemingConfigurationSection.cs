using System.Configuration;

namespace CloudBin.Web.Core.Configuration
{
    public sealed class ThemingConfigurationSection : ConfigurationSection
    {
        public const string SectionLocation = "cloudbin/theming";

        [ConfigurationProperty("themes", IsDefaultCollection = true, IsRequired = true)]
        public ThemeConfigurationElementCollection Locales
        {
            get { return (ThemeConfigurationElementCollection)this["themes"]; }
        }
    }
}
