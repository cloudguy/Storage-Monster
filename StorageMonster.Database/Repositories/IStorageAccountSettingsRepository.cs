using System;
using System.Collections.Generic;
using StorageMonster.Domain;

namespace StorageMonster.Database.Repositories
{
    public interface IStorageAccountSettingsRepository
    {
        StorageAccountSetting LoadByName(string name, int storageAccountId);
        IEnumerable<StorageAccountSetting> GetSettingsForStoargeAccount(int storageAccountId);
        StorageAccountSetting Update(StorageAccountSetting setting);
        StorageAccountSetting Create(StorageAccountSetting setting);
        UpdateResult SaveSettings(IDictionary<string, string> storageAccountSettingsList, int storageAccountId, DateTime storgaeAccountStamp);
    }
}
