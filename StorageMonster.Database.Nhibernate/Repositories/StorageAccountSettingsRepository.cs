using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Database.Repositories;

namespace StorageMonster.Database.Nhibernate.Repositories
{
    public class StorageAccountSettingsRepository : IStorageAccountSettingsRepository
    {
        public Domain.StorageAccountSetting LoadByName(string name, int storageAccountId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Domain.StorageAccountSetting> GetSettingsForStorageAccount(int storageAccountId)
        {
            throw new NotImplementedException();
        }

        public Domain.StorageAccountSetting Update(Domain.StorageAccountSetting setting)
        {
            throw new NotImplementedException();
        }

        public Domain.StorageAccountSetting Create(Domain.StorageAccountSetting setting)
        {
            throw new NotImplementedException();
        }

        public UpdateResult SaveSettings(IDictionary<string, string> storageAccountSettingsList, int storageAccountId, DateTime storageAccountStamp)
        {
            throw new NotImplementedException();
        }

        public void DeleteSettings(int storageAccountId)
        {
            throw new NotImplementedException();
        }
    }
}
