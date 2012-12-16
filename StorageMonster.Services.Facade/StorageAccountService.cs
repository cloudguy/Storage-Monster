using StorageMonster.Common;
using StorageMonster.Database;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace StorageMonster.Services.Facade
{
    public class StorageAccountService : IStorageAccountService
    {
        private readonly IStorageAccountRepository _storageAccountRepository;
        private readonly IStoragePluginsService _storagePluginsService;
        private readonly IStorageAccountSettingsRepository _storageAccountSettingsRepository;

        public StorageAccountService(IStorageAccountRepository storageAccountRepository, 
            IStoragePluginsService storagePluginsService,
            IStorageAccountSettingsRepository storageAccountSettingsRepository)
        {
            _storageAccountRepository = storageAccountRepository;
            _storagePluginsService = storagePluginsService;
            _storageAccountSettingsRepository = storageAccountSettingsRepository;
        }

//        public IEnumerable<Tuple<StorageAccount, StoragePlugin>> GetActiveStorageAccounts(int userId)
//        {
//            return _storageAccountRepository.GetAccounts(userId, (int)StorageStatus.Loaded);
//        }

//        public StorageAccount Load(int id)
//        {
//            return _storageAccountRepository.Load(id);
//        }

//        public StorageAccountCreationResult CreateStorageAccount(StorageAccount account)
//        {
//            if (account == null)
//                throw new ArgumentNullException("account");

//            return _connectionProvider.DoInTransaction(() =>
//            {
//                if (_storagePluginsService.GetStoragePlugin(account.StoragePluginId) == null)
//                    return StorageAccountCreationResult.PluginNotFound;

//                StorageAccount checkAccount = _storageAccountRepository.Load(account.AccountName, account.StoragePluginId, account.UserId);

//                if (checkAccount != null)
//                    return StorageAccountCreationResult.AccountExists;

//                _storageAccountRepository.Insert(account);
               
//                return StorageAccountCreationResult.Success;
//            });
//        }

//        public void SaveSettings(IDictionary<string, string> storageAccountSettingsList, int storageAccountId, DateTime storageAccountStamp)
//        {
//            UpdateResult updateResult = _storageAccountSettingsRepository.SaveSettings(storageAccountSettingsList, storageAccountId, storageAccountStamp);
//#warning make new exceptions
//            switch (updateResult)
//            {
//                case UpdateResult.Stalled:
//                    throw new StaleObjectException(string.Format(CultureInfo.InvariantCulture, "Error saving settings for storage account {0}, object stalled", storageAccountId));
//                case UpdateResult.ItemNotExists:
//                    throw new ObjectNotExistsException(string.Format(CultureInfo.InvariantCulture, "Error saving settings for storage account {0}, account not found", storageAccountId));
//            }
//        }

//        public IEnumerable<StorageAccountSetting> GetSettingsForStoargeAccount(int storageAccountId)
//        {
//            return _storageAccountSettingsRepository.GetSettingsForStorageAccount(storageAccountId);
//        }

//        public void DeleteStorageAccount(int storageAccountId)
//        {
//            _connectionProvider.DoInTransaction(() =>
//            {
//                _storageAccountSettingsRepository.DeleteSettings(storageAccountId);
//                _storageAccountRepository.Delete(storageAccountId);                
//            });
//        }

        public IEnumerable<Tuple<StorageAccount, StoragePluginDescriptor>> GetActiveStorageAccounts(int userId)
        {
            throw new NotImplementedException();
        }

        public StorageAccount Load(int id)
        {
            throw new NotImplementedException();
        }

        public StorageAccountCreationResult CreateStorageAccount(StorageAccount account)
        {
            throw new NotImplementedException();
        }

        public void SaveSettings(IDictionary<string, string> storageAccountSettingsList, int storageAccountId, DateTime storageAccountStamp)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StorageAccountSetting> GetSettingsForStoargeAccount(int storageAccountId)
        {
            throw new NotImplementedException();
        }

        public void DeleteStorageAccount(int storageAccountId)
        {
            throw new NotImplementedException();
        }
    }
}
