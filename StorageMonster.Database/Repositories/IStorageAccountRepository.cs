using System;
using System.Collections.Generic;
using StorageMonster.Domain;


namespace StorageMonster.Database.Repositories
{
    public interface IStorageAccountRepository
    {
        IEnumerable<Tuple<StorageAccount, StoragePluginDescriptor>> GetAccounts(int userId, int storageStatus);
        StorageAccount Load(int id);
        StorageAccount Load(string accountName, int storagePluginId, int userId);
        StorageAccount Insert(StorageAccount account);
        void Delete(int storageAccountId);
    }
}
