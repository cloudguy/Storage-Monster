using System;
using System.Collections.Generic;
using System.Globalization;
using System.Transactions;
using StorageMonster.Common;
using StorageMonster.Database;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;

namespace StorageMonster.Services.Facade
{
    public class StorageAccountService : IStorageAccountService
    {
        protected IStorageAccountRepository StorageAccountRepository { get; set; }
        protected IStoragePluginsService StoragePluginsService { get; set; }
        protected IStorageAccountSettingsRepository StorageAccountSettingsRepository { get; set; }

        public StorageAccountService(IStorageAccountRepository storageAccountRepository, 
            IStoragePluginsService storagePluginsService,
            IStorageAccountSettingsRepository storageAccountSettingsRepository)
        {
            StorageAccountRepository = storageAccountRepository;
            StoragePluginsService = storagePluginsService;
            StorageAccountSettingsRepository = storageAccountSettingsRepository;
        }

        public IEnumerable<Tuple<StorageAccount, StoragePlugin>> GetActiveStorageAccounts(int userId)
        {
            return StorageAccountRepository.GetAccounts(userId, (int)StorageStatus.Loaded);
        }

        public StorageAccount Load(int id)
        {
            return StorageAccountRepository.Load(id);
        }

        public StorageAccountCreationResult CreateStorageAccount(StorageAccount account)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                if (StoragePluginsService.GetStoragePlugin(account.StoragePluginId) == null)
                    return StorageAccountCreationResult.PluginNotFound;

                StorageAccount checkAccount = StorageAccountRepository.Load(account.AccountName, account.StoragePluginId, account.UserId);

                if (checkAccount != null)
                    return StorageAccountCreationResult.AccountExists;

                StorageAccountRepository.Insert(account);

                transactionScope.Complete();
                return StorageAccountCreationResult.Success;
            }
        }

        public void SaveSettings(IDictionary<string, string> storageAccountSettingsList, int storageAccountId, DateTime storageAccountStamp)
        {
            UpdateResult updateResult = StorageAccountSettingsRepository.SaveSettings(storageAccountSettingsList, storageAccountId, storageAccountStamp);

            switch (updateResult)
            {
                case UpdateResult.Stalled:
                    throw new StaleObjectException(string.Format(CultureInfo.InvariantCulture, "Error saving settings for storage account {0}, object stalled", storageAccountId));
                case UpdateResult.ItemNotExists:
                    throw new ObjectNotExistsException(string.Format(CultureInfo.InvariantCulture, "Error saving settings for storage account {0}, account not found", storageAccountId));
            }
        }

        public IEnumerable<StorageAccountSetting> GetSettingsForStoargeAccount(int storageAccountId)
        {
            return StorageAccountSettingsRepository.GetSettingsForStoargeAccount(storageAccountId);
        }

        public void DeleteStorageAccount(int storageAccountId)
        {
            using (var scope = new TransactionScope())
            {
                StorageAccountSettingsRepository.DeleteSettings(storageAccountId);
                StorageAccountRepository.Delete(storageAccountId);
                scope.Complete();
            }
        }
    }
}
