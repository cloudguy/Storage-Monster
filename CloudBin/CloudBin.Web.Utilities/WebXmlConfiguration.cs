using System;

namespace CloudBin.Web.Utilities
{
    public sealed class WebXmlConfiguration : IWebConfiguration
    {
        private static Lazy<WebConfigurationSection> _configSection = new Lazy<WebConfigurationSection>(() =>
        {
            return (WebConfigurationSection) System.Configuration.ConfigurationManager.GetSection(WebConfigurationSection.SectionLocation);
        }, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        #region IWebConfiguration implementation

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

        #endregion
    }
}
