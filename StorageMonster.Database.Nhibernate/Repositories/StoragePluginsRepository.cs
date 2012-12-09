using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Database.Repositories;

namespace StorageMonster.Database.Nhibernate.Repositories
{
    public class StoragePluginsRepository : IStoragePluginsRepository
    {
        public void SetStoragesStatuses(int status)
        {
            throw new NotImplementedException();
        }

        public Domain.StoragePlugin InitPluginStatus(string classPath, int status)
        {
            throw new NotImplementedException();
        }
    }
}
