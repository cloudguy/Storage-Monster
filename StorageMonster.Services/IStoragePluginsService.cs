using System.Collections.Generic;
using StorageMonster.Domain;
using StorageMonster.Plugin;

namespace StorageMonster.Services
{
    public interface IStoragePluginsService
    {
        void ResetStorages();
        void InitStorges(IEnumerable<IStoragePlugin> storagePlugins);
        IStoragePlugin GetStoragePlugin(int storagePluginId);
        IEnumerable<StoragePlugin> GetAvailableStoragePlugins();
    }
}
