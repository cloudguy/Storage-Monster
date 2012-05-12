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
    internal class StoragePluginProvider : IStoragePluginProvider
    {
        protected ICacheService CacheService;
        protected IStorageRepository StorageRepository;
        protected const string CacheKeyPrefix = "storageplugin_";

        public StoragePluginProvider(ICacheService cacheService, IStorageRepository storageRepository)
        {
            CacheService = cacheService;
            StorageRepository = storageRepository;
        }

        public IStoragePlugin GetPlugin(int pluginId)
        {
            string cacheKey = GetStorageCacheKey(pluginId);
            Type pluginType = CacheService.Get<Type>(cacheKey);
            if (pluginType == null)
                return null;
            return (IStoragePlugin)IoCcontainer.Instance.Resolve(pluginType);
        }

        public void SetPlugin(int pluginId, IStoragePlugin storagePlugin)
        {
            string cacheKey = GetStorageCacheKey(pluginId);
            Type pluginType = storagePlugin.GetType();
            CacheService.Set(cacheKey, pluginType);
        }

        protected static string GetStorageCacheKey(int storageId)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}{1}", CacheKeyPrefix, storageId);
        }
    }
}
