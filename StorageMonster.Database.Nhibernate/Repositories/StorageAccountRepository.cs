using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Database.Repositories;

namespace StorageMonster.Database.Nhibernate.Repositories
{
    public class StorageAccountRepository : IStorageAccountRepository
    {
        public IEnumerable<Tuple<Domain.StorageAccount, Domain.StoragePlugin>> GetAccounts(int userId, int storageStatus)
        {
            throw new NotImplementedException();
        }

        public Domain.StorageAccount Load(int id)
        {
            throw new NotImplementedException();
        }

        public Domain.StorageAccount Load(string accountName, int storagePluginId, int userId)
        {
            throw new NotImplementedException();
        }

        public Domain.StorageAccount Insert(Domain.StorageAccount account)
        {
            throw new NotImplementedException();
        }

        public void Delete(int storageAccountId)
        {
            throw new NotImplementedException();
        }
    }
}
