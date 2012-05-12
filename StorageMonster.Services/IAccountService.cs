using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.DB.Domain;
using StorageMonster.Util;

namespace StorageMonster.Services
{
    public interface IAccountService
    {
        IEnumerable<Tuple<Account, Storage>> GetActiveAccounts(int userId);
    }
}
