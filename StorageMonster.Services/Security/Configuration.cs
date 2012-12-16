using System;
using System.Configuration;
using System.Text;

namespace StorageMonster.Services.Security
{
    public class Configuration : ConfigurationSection
    {
        public const string SectionLocation = "monster/security";

        [ConfigurationProperty("encryption")]
        public EncryptionElement Encryption
        {
            get { return (EncryptionElement)this["encryption"]; }
            set { this["encryption"] = value; }
        }

        public class EncryptionElement : ConfigurationElement
        {
            [ConfigurationProperty("key", DefaultValue = "37d3a5e4-2a5f-4421-ba5e-8773c3a27958", IsRequired = true)]
            public String Key
            {
                get { return (String)this["key"]; }
                set { this["key"] = value; }
            }

            [ConfigurationProperty("salt", DefaultValue = "55ad57e7-54c2-4f09-836c-66c66014be1b", IsRequired = true)]
            public String SaltAsString
            {
                get { return (String)this["salt"]; }
                set { this["salt"] = value; }
            }

            public byte[] Salt
            {
                get { return Encoding.ASCII.GetBytes(SaltAsString); }
                set { SaltAsString = Encoding.ASCII.GetString(value); }
            }
        }
    }
}
