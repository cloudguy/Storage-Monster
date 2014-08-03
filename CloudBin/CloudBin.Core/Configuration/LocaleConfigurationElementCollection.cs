using System.Configuration;

namespace CloudBin.Core.Configuration
{
    [ConfigurationCollection(typeof (LocaleConfigurationElement))]
    public sealed class LocaleConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LocaleConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LocaleConfigurationElement) element).Name;
        }
    }
}
