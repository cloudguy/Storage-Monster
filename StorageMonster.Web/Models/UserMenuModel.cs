using System.Collections.Generic;

namespace StorageMonster.Web.Models
{
    public class UserMenuModel
    {
        public class AccountItem
        {
            public int AccountId { get; set; }
            public int StorageId { get; set; }
            public string StorageName { get; set; }
            public string AccountLogin { get; set; }
            public string AccountServer { get; set; }
        }

        public IEnumerable<AccountItem> Accounts { get; set; }
        public int UserId { get; set; }
    }
}