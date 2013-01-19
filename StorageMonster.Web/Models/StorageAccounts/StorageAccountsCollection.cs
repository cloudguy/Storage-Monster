using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageMonster.Web.Models.StorageAccounts
{
    public class StorageAccountsCollection
    {
        public IEnumerable<StorageAccountModel> Accounts { get; set; }
    }
}