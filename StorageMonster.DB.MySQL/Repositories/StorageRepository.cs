using System;
using System.Collections.Generic;
using StorageMonster.DB.Repositories;
using StorageMonster.DB.Domain;

namespace StorageMonster.DB.MySQL.Repositories
{
	public class StorageRepository : IStorageRepository
	{
		protected IConnectionProvider Connectionprovider;

		public StorageRepository(IConnectionProvider connectionprovider)
		{
			Connectionprovider = connectionprovider;
		}

		public StorageMonster.DB.Domain.Storage Load(int id)
		{
			throw new NotImplementedException();
		}

		public StorageMonster.DB.Domain.Storage Create(StorageMonster.DB.Domain.Storage plugin)
		{
			throw new NotImplementedException();
		}

		public StorageMonster.DB.Domain.Storage Update(StorageMonster.DB.Domain.Storage plugin)
		{
			throw new NotImplementedException();
		}

		public void Delete(StorageMonster.DB.Domain.Storage plugin)
		{
			throw new NotImplementedException();
		}

		public void Delete(int id)
		{
            throw new NotImplementedException();
		}

		public IEnumerable<Storage> List()
		{
            return Connectionprovider.CurrentConnection
                .Query<Storage>("select id as 'Id', classpath as 'ClassPath', status as 'Status', stamp as 'Stamp' from storages", null);
		}
	}
}

