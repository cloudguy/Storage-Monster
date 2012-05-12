using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.DB.Domain;
using StorageMonster.Util;

namespace StorageMonster.DB.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Tuple<Account, Storage>> GetAccounts(int userId, int storageStatus);
    }
}
