using System.Configuration;

namespace CloudBin.Core.Configuration
{
    public sealed class GlobalizationConfigurationSection : ConfigurationSection
    {
        public const string SectionLocation = "cloudbin/globalization";

        [ConfigurationProperty("locales", IsDefaultCollection = true, IsRequired = true)]
        public LocaleConfigurationElementCollection Locales
        {
            get { return (LocaleConfigurationElementCollection)this["locales"]; }
        }
    }
}
