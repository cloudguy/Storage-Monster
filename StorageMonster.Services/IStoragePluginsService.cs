using StorageMonster.Domain;
using StorageMonster.Plugin;
using System.Collections.Generic;

namespace StorageMonster.Services
{
    public interface IStoragePluginsService
    {
        void ResetStorages();
        void InitStorges(IEnumerable<IStoragePlugin> storagePlugins);
        IStoragePlugin GetStoragePlugin(int storagePluginId);
        IEnumerable<StoragePluginDescriptor> GetAvailableStoragePlugins();
        StorageFileStreamResult DownloadFile(StorageAccount storageAccount, string url);
    }
}
