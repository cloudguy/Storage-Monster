using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using StorageMonster.Common;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;
using StorageMonster.Plugin;

namespace StorageMonster.Services.Facade
{
    public class StoragePluginsService : IStoragePluginsService
    {
        protected IStoragePluginsRepository StoragePluginsRepository;
		protected ICacheService CacheService;
        protected const string PluginsCacheKey = "storage_plugins_cache";

        protected static ConcurrentBag<CachedStoragePlugin> CachedStoragePlugins = new ConcurrentBag<CachedStoragePlugin>();

        protected class CachedStoragePlugin
        {
            public StoragePlugin PluginDescriptor { get; set; }
            public IStoragePlugin Plugin { get; set; }

            public CachedStoragePlugin(StoragePlugin pluginDescriptor, IStoragePlugin plugin)
            {
                PluginDescriptor = pluginDescriptor;
                Plugin = plugin;
            }
        }

		public StoragePluginsService(IStoragePluginsRepository storageRepository, ICacheService cacheService)
        {
            StoragePluginsRepository = storageRepository;
			CacheService = cacheService;
        }

        public void InitStorges(IEnumerable<IStoragePlugin> storagePlugins)
        {
            foreach (var storagePlugin in storagePlugins)
            {
                //updating db
                StoragePlugin pluginDescriptor = StoragePluginsRepository.InitPluginStatus(storagePlugin.GetType().FullName, (int)StorageStatus.Loaded);
                CachedStoragePlugins.Add(new CachedStoragePlugin(pluginDescriptor, storagePlugin));
            }
        }

        public IEnumerable<StoragePlugin> GetAvailableStoragePlugins()
        {
            return CachedStoragePlugins.Select((p) => p.PluginDescriptor);
        }

        public void ResetStorages()
        {
            StoragePluginsRepository.SetStoragesStauses((int)StorageStatus.Unloaded);
            CachedStoragePlugin result;
            while (!CachedStoragePlugins.IsEmpty)
                CachedStoragePlugins.TryTake(out result);
            
        }

        public IStoragePlugin GetStoragePlugin(int pluginId)
        {
            CachedStoragePlugin cachedStoragePlugin = CachedStoragePlugins.FirstOrDefault((p) => p.PluginDescriptor.Id == pluginId);
            return cachedStoragePlugin == null ? null : cachedStoragePlugin.Plugin;
        }
    }
}


