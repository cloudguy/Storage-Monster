using System.Configuration;

namespace CloudBin.Core.Configuration
{
    public class DependencyContainerConfigurationSection : ConfigurationSection
    {
        public const string SectionLocation = "cloudbin/dependencyContainer";

        [ConfigurationProperty("type", IsRequired = true)]
        public string ContainerType
        {
            get { return (string)this["type"]; }
        }
    }
}
