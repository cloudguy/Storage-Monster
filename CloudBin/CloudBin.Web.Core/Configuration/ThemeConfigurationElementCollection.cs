using System.Configuration;

namespace CloudBin.Web.Core.Configuration
{
    [ConfigurationCollection(typeof (ThemeConfigurationElement))]
    public sealed class ThemeConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ThemeConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ThemeConfigurationElement) element).Name;
        }
    }
}
