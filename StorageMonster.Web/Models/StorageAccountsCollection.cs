using System.Collections.Generic;
using System.Linq;
using System.Web;
using StorageMonster.Services;
using StorageMonster.Web.Services.Security;


namespace StorageMonster.Web.Models
{
    public class StorageAccountsCollection
    {
        public class AccountItem
        {
            public int AccountId { get; set; }
            public int StoragePluginId { get; set; }
            public string StoragePluginName { get; set; }
            public string AccountName { get; set; }
        }

        public IEnumerable<AccountItem> Accounts { get; set; }
        public int UserId { get; set; }

        public StorageAccountsCollection Init(IStorageAccountService storageAccountService, IStoragePluginsService storagePluginsService, int userId)
        {
            var accounts = storageAccountService.GetActiveStorageAccounts(userId);
            UserId = userId;
            Accounts = accounts.Where(a=>storagePluginsService.GetStoragePlugin(a.Item2.Id) != null)
                .Select(a => new AccountItem()
                {
                    AccountId = a.Item1.Id,
                    AccountName = a.Item1.AccountName,
                    StoragePluginId = a.Item1.StoragePluginId,
                    StoragePluginName = storagePluginsService.GetStoragePlugin(a.Item2.Id).Name
                });
            return this;
        }
    }
}