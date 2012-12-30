using System;
using System.Configuration;

namespace StorageMonster.Web.Services.Configuration
{
    public class WebConfigurationSection : ConfigurationSection
    {
        public const string SectionLocation = "monster/web";

        [ConfigurationProperty("clientObjects")]
        public ClientObjectsElement ClientObjects
        {
            get { return (ClientObjectsElement) this["clientObjects"]; }
            set { this["clientObjects"] = value; }
        }

        [ConfigurationProperty("sweeper")]
        public SweeperElement Sweeper
        {
            get { return (SweeperElement)this["sweeper"]; }
            set { this["sweeper"] = value; }
        }

        [ConfigurationProperty("tracking")]
        public TrackingElement Tracking
        {
            get { return (TrackingElement)this["tracking"]; }
            set { this["tracking"] = value; }
        }

        public class TrackingElement : ConfigurationElement
        {
            [ConfigurationProperty("cookieName", DefaultValue = "sm_track", IsRequired = false)]
            public String CookieName
            {
                get { return (String)this["cookieName"]; }
                set { this["cookieName"] = value; }
            }

            [ConfigurationProperty("cookieExpiration", DefaultValue = "527040", IsRequired = false)]
            public int CookieExpiration
            {
                get
                {
                    int value = (int)this["cookieExpiration"];
                    return value <= 0 ? 527040 : value;
                }
                set { this["cookieExpiration"] = value; }
            }
        }

        public class ClientObjectsElement : ConfigurationElement
        {
            [ConfigurationProperty("jsVersion", DefaultValue = "v1.0", IsRequired = false)]
            public String JsVersion
            {
                get { return (String) this["jsVersion"]; }
                set { this["jsVersion"] = value; }
            }

            [ConfigurationProperty("cssVersion", DefaultValue = "v1.0", IsRequired = false)]
            public String CssVersion
            {
                get { return (String)this["cssVersion"]; }
                set { this["cssVersion"] = value; }
            }
        }

        public class SweeperElement : ConfigurationElement
        {
            [ConfigurationProperty("enabled", DefaultValue = "False", IsRequired = false)]
            public Boolean Enabled
            {
                get { return (Boolean)this["enabled"]; }
                set { this["enabled"] = value; }
            }

            [ConfigurationProperty("timeout", DefaultValue = "30", IsRequired = false)]
            public int SweeperTimeout
            {
                get
                {
                    int value = (int)this["timeout"];
                    return value <= 0 ? 30 : value;
                }
                set { this["timeout"] = value; }
            }
        }
    }
}