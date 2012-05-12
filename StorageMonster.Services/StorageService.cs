using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using StorageMonster.Common;
using StorageMonster.DB.Repositories;
using StorageMonster.Plugin;

namespace StorageMonster.Services
{
    internal class StorageService : IStorageService
    {
        protected IStorageRepository StorageRepository;
        protected IStoragePluginProvider StoragePluginProvider;

        public StorageService(IStorageRepository storageRepository, IStoragePluginProvider storagePluginProvider)
        {
            StorageRepository = storageRepository;
            StoragePluginProvider = storagePluginProvider;
        }

        public void InitStorges(IEnumerable<IStoragePlugin> storagePlugins)
        {
            foreach (var storagePlugin in storagePlugins)
            {
                //updating db
                int pluginId = StorageRepository.InitPluginStatus(storagePlugin.GetType().FullName, (int)StorageStatus.Loaded);
                StoragePluginProvider.SetPlugin(pluginId, storagePlugin);
            }
        }

        public void ResetStorages()
        {
            StorageRepository.SetStoragesStauses((int)StorageStatus.Unloaded);
        }

        public IStoragePlugin GetStoragePlugin(int pluginId)
        {
            return StoragePluginProvider.GetPlugin(pluginId);
        }
    }
}
