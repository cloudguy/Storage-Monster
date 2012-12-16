using System;
using System.Collections.Generic;
using StorageMonster.Domain;


namespace StorageMonster.Services
{
    public enum StorageAccountCreationResult
    {
        Success,
        AccountExists,
        PluginNotFound
    }
    public interface IStorageAccountService
    {
        IEnumerable<Tuple<StorageAccount, StoragePluginDescriptor>> GetActiveStorageAccounts(int userId);
        StorageAccount Load(int id);
        StorageAccountCreationResult CreateStorageAccount(StorageAccount account);
        void SaveSettings(IDictionary<string, string> storageAccountSettingsList, int storageAccountId, DateTime storageAccountStamp);
        IEnumerable<StorageAccountSetting> GetSettingsForStoargeAccount(int storageAccountId);
        void DeleteStorageAccount(int storageAccountId);
    }
}
