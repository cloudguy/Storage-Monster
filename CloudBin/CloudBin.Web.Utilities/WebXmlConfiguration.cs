using System;

namespace CloudBin.Web.Utilities
{
    public sealed class WebXmlConfiguration : IWebConfiguration
    {
        private static readonly Lazy<WebConfigurationSection> _configSection = new Lazy<WebConfigurationSection>(() =>
        {
            return (WebConfigurationSection) System.Configuration.ConfigurationManager.GetSection(WebConfigurationSection.SectionLocation);
        }, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
       
        bool IWebConfiguration.DoNotOpenDbSessionForScriptAndContent
        {
            get { return _configSection.Value.DoNotOpenDbSessionForScriptAndContent; }
        }

        bool IWebConfiguration.CompressDynamicContent
        {
            get { return _configSection.Value.CompressDynamicContent; }
        }

        bool IWebConfiguration.CompressStaticContent
        {
            get { return _configSection.Value.CompressStaticContent; }
        }

        bool IWebConfiguration.SendSecurityHeaders
        {
            get { return _configSection.Value.SendSecurityHeaders; }
        }

        bool IWebConfiguration.RemoveVersionHeaders
        {
            get { return _configSection.Value.RemoveVersionHeaders; }
        }
    }
}
