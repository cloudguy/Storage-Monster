using System;
using System.Configuration;

namespace StorageMonster.Web.Services.Security
{
    public class AuthConfigurationSection : ConfigurationSection
    {
        public const string SectionLocation = "monster/auth";

        [ConfigurationProperty("cookieauth")]
        public CookieAuthConfigurationObject CookieAuth
        {
            get { return (CookieAuthConfigurationObject)this["cookieauth"]; }
            set { this["cookieauth"] = value; }
        }

        [ConfigurationProperty("membership")]
        public MembershipConfigurationObject Memebership
        {
            get { return (MembershipConfigurationObject)this["membership"]; }
            set { this["membership"] = value; }
        }

        public class MembershipConfigurationObject : ConfigurationElement
        {
            public int MaxPasswordLength { get { return 100; } }
            public int MaxUserNameLength { get { return 100; } }
            public int MaxEmailLength { get { return 100; } }

            [ConfigurationProperty("resetPasswdExpiration", DefaultValue = "30", IsRequired = false)]
            public int ResetPasswordRequestExpiration
            {
                get
                {
                    int value = (int)this["resetPasswdExpiration"];
                    return value <= 0 ? 30 : value;
                }
                set { this["resetPasswdExpiration"] = value; }
            }

            [ConfigurationProperty("resetPasswdMailFrom", DefaultValue = "do-not-reply@storage-monster.com", IsRequired = false)]
            public string RestorePasswordMailFrom
            {
                get { return (string)this["resetPasswdMailFrom"]; }
                set { this["resetPasswdMailFrom"] = value; }
            }

            [ConfigurationProperty("userNameRegexp", DefaultValue = "^[a-zA-Z1-9 ]{1,100}$", IsRequired = false)]
            public string UserNameRegexp
            {
                get { return (string)this["userNameRegexp"]; }
                set { this["userNameRegexp"] = value; }
            }

            [ConfigurationProperty("emailRegexp", DefaultValue = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", IsRequired = false)]
            public string EmailRegexp
            {
                get { return (string)this["emailRegexp"]; }
                set { this["emailRegexp"] = value; }
            }

            [ConfigurationProperty("minPasswordLength", DefaultValue = "6", IsRequired = false)]
            public int MinPasswordLength
            {
                get
                {
                    int value = (int)this["minPasswordLength"];
                    return value <= 0 ? 6 : value;
                }
                set { this["minPasswordLength"] = value; }
            }
        }

        public class CookieAuthConfigurationObject : ConfigurationElement
        {
            [ConfigurationProperty("allowMultipleLogons", DefaultValue = "False", IsRequired = false)]
            public Boolean AllowMultipleLogons
            {
                get { return (Boolean)this["allowMultipleLogons"]; }
                set { this["allowMultipleLogons"] = value; }
            }

            [ConfigurationProperty("cookieName", DefaultValue = "sm_auth", IsRequired = false)]
            public string AuthenticationCookieName
            {
                get { return (string)this["cookieName"]; }
                set { this["cookieName"] = value; }
            }

            [ConfigurationProperty("expiration", DefaultValue = "30", IsRequired = false)]
            public int AuthenticationExpiration
            {
                get
                {
                    int value = (int)this["expiration"];
                    return value <= 0 ? 30 : value;
                }
                set { this["expiration"] = value; }
            }

            [ConfigurationProperty("slidingExpiration", DefaultValue = "True", IsRequired = false)]
            public Boolean AuthenticationSlidingExpiration
            {
                get { return (Boolean)this["slidingExpiration"]; }
                set { this["slidingExpiration"] = value; }
            }
        }
    }
}