using System;
using StorageMonster.Database;
using System.IO;

namespace StorageMonster.Plugin
{
    public interface IStoragePlugin
    {
        string Name { get; }
        object GetAccountConfigurationModel(int accountId);
        object GetAccountConfigurationModel();
        void ApplyConfiguration(int accountId, DateTime accountStamp, object configurationModel);
        StorageFolderResult QueryStorage(int accountId, string path);
        StorageFileStreamResult GetFileStream(string fileUrl, int accountId);
    }
}
