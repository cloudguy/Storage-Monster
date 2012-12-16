using StorageMonster.Domain;

namespace StorageMonster.Database.Repositories
{
	public interface IStoragePluginDescriptorsRepository
	{
        void SetStoragesStatuses(StoragePluginStatus status);
	    StoragePluginDescriptor GetPluginByClassPath(string classPath);
        StoragePluginDescriptor Insert(StoragePluginDescriptor pluginDescriptor);
        void Update(StoragePluginDescriptor pluginDescriptor);
        void SaveOrUpdate(StoragePluginDescriptor pluginDescriptor);
	}
}
