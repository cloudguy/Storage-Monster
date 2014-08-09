using System;

namespace CloudBin.Web.Utilities.Configuration
{
    public sealed class WebXmlConfiguration : IWebConfiguration
    {
        private static readonly Lazy<WebConfigurationSection> ConfigSection = new Lazy<WebConfigurationSection>(() =>
        {
            return (WebConfigurationSection) System.Configuration.ConfigurationManager.GetSection(WebConfigurationSection.SectionLocation);
        }, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
       
        bool IWebConfiguration.DoNotOpenDbSessionForScriptAndContent
        {
            get { return ConfigSection.Value.DoNotOpenDbSessionForScriptAndContent; }
        }

        bool IWebConfiguration.CompressDynamicContent
        {
            get { return ConfigSection.Value.CompressDynamicContent; }
        }

        bool IWebConfiguration.CompressStaticContent
        {
            get { return ConfigSection.Value.CompressStaticContent; }
        }

        bool IWebConfiguration.SendSecurityHeaders
        {
            get { return ConfigSection.Value.SendSecurityHeaders; }
        }

        bool IWebConfiguration.RemoveVersionHeaders
        {
            get { return ConfigSection.Value.RemoveVersionHeaders; }
        }
    }
}
