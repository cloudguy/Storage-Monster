using System.Configuration;

namespace CloudBin.Web.Utilities
{
    public sealed class WebConfigurationSection : ConfigurationSection
    {
        public const string SectionLocation = "cloudbin/web";

        [ConfigurationProperty("doNotOpenDbSessionForScriptAndContent", DefaultValue = "True", IsRequired = false)]
        public bool DoNotOpenDbSessionForScriptAndContent
        {
            get { return (bool) this["doNotOpenDbSessionForScriptAndContent"]; }
        }

        [ConfigurationProperty("compressDynamicContent", DefaultValue = "False", IsRequired = false)]
        public bool CompressDynamicContent
        {
            get { return (bool) this["compressDynamicContent"]; }
        }

        [ConfigurationProperty("compressStaticContent", DefaultValue = "False", IsRequired = false)]
        public bool CompressStaticContent
        {
            get { return (bool) this["compressStaticContent"]; }
        }

        [ConfigurationProperty("sendSecurityHeaders", DefaultValue = "True", IsRequired = false)]
        public bool SendSecurityHeaders
        {
            get { return (bool) this["sendSecurityHeaders"]; }
        }

        [ConfigurationProperty("removeVersionHeaders", DefaultValue = "True", IsRequired = false)]
        public bool RemoveVersionHeaders
        {
            get { return (bool)this["removeVersionHeaders"]; }
        }
    }
}
