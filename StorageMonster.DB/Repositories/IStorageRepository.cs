using StorageMonster.DB.Domain;
using System.Collections.Generic;

namespace StorageMonster.DB.Repositories
{
	public interface IStorageRepository
	{
        void SetStoragesStauses(int status);
        int InitPluginStatus(string classPath, int status);
	}
}
