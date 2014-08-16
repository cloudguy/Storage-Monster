using System.Configuration;

namespace CloudBin.Web.Core.Configuration
{
    public sealed class ThemeConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string) this["name"]; }
        }

        [ConfigurationProperty("isDefault", IsRequired = false, DefaultValue = "False")]
        public bool IsDefault
        {
            get { return (bool) this["isDefault"]; }
        }
    }
}
