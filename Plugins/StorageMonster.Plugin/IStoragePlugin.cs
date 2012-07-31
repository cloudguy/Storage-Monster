using System;
using StorageMonster.Database;

namespace StorageMonster.Plugin
{
    public interface IStoragePlugin
    {
        string Name { get; }
        object GetAccountConfigurationModel(int accountId);
        object GetAccountConfigurationModel();
        void ApplyConfiguration(int accountId, DateTime accountStamp, object configurationModel);
        StorageQueryResult QueryStorage(int accountId, string path);
    }
}
