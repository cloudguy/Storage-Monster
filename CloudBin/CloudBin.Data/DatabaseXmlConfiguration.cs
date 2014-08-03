using System;

namespace CloudBin.Data
{
    public sealed class DatabaseXmlConfiguration : IDatabaseConfiguration
    {
        private static Lazy<DatabaseConfigurationSection> _configSection = new Lazy<DatabaseConfigurationSection>(() =>
        {
            return (DatabaseConfigurationSection) System.Configuration.ConfigurationManager.GetSection(DatabaseConfigurationSection.SectionLocation);
        }, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        #region IDatabaseConfiguration implementation

        bool IDatabaseConfiguration.UseOptimisticLockForUsers
        {
            get { return _configSection.Value.UseOptimisticLockForUsers; }
        }

        bool IDatabaseConfiguration.UseOptimisticLockForStorageAccounts
        {
            get { return _configSection.Value.UseOptimisticLockForStorageAccounts; }
        }

        #endregion
    }
}
