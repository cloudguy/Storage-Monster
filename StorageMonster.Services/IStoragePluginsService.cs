using System.Collections.Generic;
using StorageMonster.Domain;
using StorageMonster.Plugin;
using System.IO;

namespace StorageMonster.Services
{
    public interface IStoragePluginsService
    {
        void ResetStorages();
        void InitStorges(IEnumerable<IStoragePlugin> storagePlugins);
        IStoragePlugin GetStoragePlugin(int storagePluginId);
        IEnumerable<StoragePlugin> GetAvailableStoragePlugins();
        StorageFileStreamResult DownloadFile(StorageAccount storageAccount, string url);
    }
}
