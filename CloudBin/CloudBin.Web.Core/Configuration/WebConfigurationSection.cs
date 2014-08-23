using System.Configuration;

namespace CloudBin.Web.Core.Configuration
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

        [ConfigurationProperty("compressBundledContent", DefaultValue = "False", IsRequired = false)]
        public bool CompressBundledContent
        {
            get { return (bool)this["compressBundledContent"]; }
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

        [ConfigurationProperty("minifyHtml", DefaultValue = "True", IsRequired = false)]
        public bool MinifyHtml
        {
            get { return (bool)this["minifyHtml"]; }
        }

        [ConfigurationProperty("trackingCookieName", DefaultValue = "tracking", IsRequired = false)]
        public string TrackingCookieName
        {
            get { return (string)this["trackingCookieName"]; }
        }
    }
}
