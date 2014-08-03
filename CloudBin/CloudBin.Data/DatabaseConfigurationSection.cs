using System;
using System.Configuration;

namespace CloudBin.Data
{
    public sealed class DatabaseConfigurationSection : ConfigurationSection
    {
        public const string SectionLocation = "cloudbin/database";

        [ConfigurationProperty("useOptimisticLockForUsers", DefaultValue = "true", IsRequired = false)]
        public bool UseOptimisticLockForUsers
        {
            get { return (Boolean) this["useOptimisticLockForUsers"]; }
        }

        [ConfigurationProperty("useOptimisticLockForStorageAccounts", DefaultValue = "true", IsRequired = false)]
        public bool UseOptimisticLockForStorageAccounts
        {
            get { return (Boolean) this["useOptimisticLockForStorageAccounts"]; }
        }
    }
}
