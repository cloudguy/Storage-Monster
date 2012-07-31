using StorageMonster.Domain;

namespace StorageMonster.Database.Repositories
{
	public interface IStoragePluginsRepository
	{
        void SetStoragesStauses(int status);
        StoragePlugin InitPluginStatus(string classPath, int status);
	}
}
