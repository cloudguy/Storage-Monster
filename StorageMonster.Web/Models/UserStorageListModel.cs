using System.Collections.Generic;

namespace StorageMonster.Web.Models
{
    public class UserStorageListModel
    {
        public IEnumerable<UserStorageModel> UserStorages { get; set; }
        public IEnumerable<UserMenuModel> UserAccounts { get; set; }
    }
}