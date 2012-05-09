using StorageMonster.DB.Domain;
using System.Collections.Generic;

namespace StorageMonster.DB.Repositories
{
	public interface IStorageRepository
	{
		Storage Load(int id);
		Storage Create(Storage plugin);
		Storage Update(Storage plugin);
		void Delete(Storage plugin);
		void Delete(int id);
		IEnumerable<Storage> List();
	}
}
