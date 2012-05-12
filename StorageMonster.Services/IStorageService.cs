using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Plugin;

namespace StorageMonster.Services
{
    public interface IStorageService
    {
        void ResetStorages();
        void InitStorges(IEnumerable<IStoragePlugin> storagePlugins);
        IStoragePlugin GetStoragePlugin(int pluginId);
    }
}
