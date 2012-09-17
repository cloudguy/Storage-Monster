using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;
using StorageMonster.Plugin;
using System.IO;

namespace StorageMonster.Services.Facade
{
    public class StoragePluginsService : IStoragePluginsService
    {
        private readonly IStoragePluginsRepository _storagePluginsRepository;
        private readonly IStreamFactory _streamFactory;

        private static readonly ConcurrentBag<CachedStoragePlugin> CachedStoragePlugins = new ConcurrentBag<CachedStoragePlugin>();

        private class CachedStoragePlugin
        {
            public StoragePlugin PluginDescriptor { get; set; }
            public IStoragePlugin Plugin { get; set; }

            public CachedStoragePlugin(StoragePlugin pluginDescriptor, IStoragePlugin plugin)
            {
                PluginDescriptor = pluginDescriptor;
                Plugin = plugin;
            }
        }

		public StoragePluginsService(IStoragePluginsRepository storageRepository, IStreamFactory streamFactory)
        {
            _storagePluginsRepository = storageRepository;
            _streamFactory = streamFactory;
        }

        public void InitStorges(IEnumerable<IStoragePlugin> storagePlugins)
        {
            if (storagePlugins == null)
                throw new ArgumentNullException("storagePlugins");

            foreach (var storagePlugin in storagePlugins)
            {
                //updating db
                StoragePlugin pluginDescriptor = _storagePluginsRepository.InitPluginStatus(storagePlugin.GetType().FullName, (int)StorageStatus.Loaded);
                CachedStoragePlugins.Add(new CachedStoragePlugin(pluginDescriptor, storagePlugin));
            }
        }

        public IEnumerable<StoragePlugin> GetAvailableStoragePlugins()
        {
            return CachedStoragePlugins.Select(p => p.PluginDescriptor);
        }

        public void ResetStorages()
        {
            _storagePluginsRepository.SetStoragesStatuses((int)StorageStatus.Unloaded);
            CachedStoragePlugin result;
            while (!CachedStoragePlugins.IsEmpty)
                CachedStoragePlugins.TryTake(out result);
        }

        public IStoragePlugin GetStoragePlugin(int storagePluginId)
        {
            CachedStoragePlugin cachedStoragePlugin = CachedStoragePlugins.FirstOrDefault(p => p.PluginDescriptor.Id == storagePluginId);
            return cachedStoragePlugin == null ? null : cachedStoragePlugin.Plugin;
        }

        public StorageFileStreamResult DownloadFile(StorageAccount storageAccount, string url)
        {
            if (storageAccount == null)
                throw new ArgumentNullException("storageAccount");
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            IStoragePlugin plugin = GetStoragePlugin(storageAccount.StoragePluginId);
            if (plugin == null)
                throw new StoragePluginNotFoundException(storageAccount.StoragePluginId);

            var streamResult = plugin.GetFileStream(url, storageAccount.Id);
            streamResult.FileStream = _streamFactory.MakeDownloadStream(streamResult.FileStream);            
            return streamResult;
        }
    }
}


