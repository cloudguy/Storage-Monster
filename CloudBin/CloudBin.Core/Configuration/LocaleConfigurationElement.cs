using System.Configuration;

namespace CloudBin.Core.Configuration
{
    public sealed class LocaleConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string) this["name"]; }
        }

        [ConfigurationProperty("culture", IsRequired = true)]
        public string Culture
        {
            get { return (string) this["culture"]; }
        }

        [ConfigurationProperty("uiCulture", IsRequired = true)]
        public string UiCulture
        {
            get { return (string) this["uiCulture"]; }
        }

        [ConfigurationProperty("fullName", IsRequired = true)]
        public string FullName
        {
            get { return (string) this["fullName"]; }
        }

        [ConfigurationProperty("isDefault", IsRequired = false, DefaultValue = "False")]
        public bool IsDefault
        {
            get { return (bool)this["isDefault"]; }
        }
    }
}
