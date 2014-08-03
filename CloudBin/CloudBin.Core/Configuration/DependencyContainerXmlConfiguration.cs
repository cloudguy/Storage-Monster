using System;

namespace CloudBin.Core.Configuration
{
    public class DependencyContainerXmlConfiguration : IDependencyContainerConfiguration
    {
        private static readonly Lazy<DependencyContainerConfigurationSection> ConfigSection = new Lazy<DependencyContainerConfigurationSection>(() =>
        {
            return (DependencyContainerConfigurationSection)System.Configuration.ConfigurationManager.GetSection(DependencyContainerConfigurationSection.SectionLocation);
        }, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        string IDependencyContainerConfiguration.DependencyContainerType
        {
            get
            {
                return ConfigSection.Value.ContainerType;
            }
        }

    }
}
