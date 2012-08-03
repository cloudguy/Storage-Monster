using System;
using System.Collections.Generic;
using StorageMonster.Database;
using StorageMonster.Domain;


namespace StorageMonster.Services
{
#warning make exceptions
    public enum StorageAccountCreationResult
    {
        Success,
        AccountExists,
        PluginNotFound
    }
    public interface IStorageAccountService
    {
        IEnumerable<Tuple<StorageAccount, StoragePlugin>> GetActiveStorageAccounts(int userId);
        StorageAccount Load(int id);
        StorageAccountCreationResult CreateStorageAccount(StorageAccount account);
        void SaveSettings(IDictionary<string, string> storageAccountSettingsList, int storageAccountId, DateTime storageAccountStamp);
        IEnumerable<StorageAccountSetting> GetSettingsForStoargeAccount(int storageAccountId);
        void DeleteStorageAccount(int storageAccountId);
    }
}
