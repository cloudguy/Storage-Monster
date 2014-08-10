using System.Configuration;

namespace CloudBin.Web.Core.Configuration
{
    public sealed class AuthenticationConfigurationSection : ConfigurationSection
    {
        public const string SectionLocation = "cloudbin/authentication";

        [ConfigurationProperty("cookieName", DefaultValue = "cb_au", IsRequired = false)]
        public string CookieName
        {
            get { return (string)this["cookieName"]; }
        }

        [ConfigurationProperty("signInUrl", IsRequired = true)]
        public string SignInUrl
        {
            get { return (string)this["signInUrl"]; }
        }

        [ConfigurationProperty("encryptionKey", IsRequired = true)]
        public string EncryptionKey
        {
            get { return (string)this["encryptionKey"]; }
        }

        [ConfigurationProperty("validationKey", IsRequired = true)]
        public string ValidationKey
        {
            get { return (string)this["validationKey"]; }
        }

        [ConfigurationProperty("slideExpire", DefaultValue = "True", IsRequired = false)]
        public bool SlideExpire
        {
            get { return (bool)this["slideExpire"]; }
        }

        [ConfigurationProperty("requireSSL", DefaultValue = "True", IsRequired = false)]
        // ReSharper disable InconsistentNaming
        public bool RequireSSL
        // ReSharper restore InconsistentNaming
        {
            get { return (bool)this["requireSSL"]; }
        }

        [ConfigurationProperty("allowMultipleSessions", DefaultValue = "True", IsRequired = false)]
        public bool AllowMultipleSessions
        {
            get { return (bool)this["allowMultipleSessions"]; }
        }

        [ConfigurationProperty("encryptionAlgorithm", DefaultValue = "rijndael", IsRequired = false)]
        public string EncryptionAlgorithm
        {
            get { return (string)this["encryptionAlgorithm"]; }
        }

        [ConfigurationProperty("validationAlgorithm", DefaultValue = "hmacsha256", IsRequired = false)]
        public string ValidationAlgorithm
        {
            get { return (string)this["validationAlgorithm"]; }
        }

        [ConfigurationProperty("doNotAuthenticateScriptAndContent", DefaultValue = "True", IsRequired = false)]
        public bool DoNotAuthenticateScriptAndContent
        {
            get { return (bool)this["doNotAuthenticateScriptAndContent"]; }
        }

        [ConfigurationProperty("sessionTimeout", DefaultValue = "60", IsRequired = false)]
        public int SessionTimeout
        {
            get { return (int)this["sessionTimeout"]; }
        }
    }
}
