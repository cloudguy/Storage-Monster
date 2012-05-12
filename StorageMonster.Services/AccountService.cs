using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.DB.Domain;
using StorageMonster.DB.Repositories;
using StorageMonster.Util;

namespace StorageMonster.Services
{
    internal class AccountService : IAccountService
    {
        protected IAccountRepository AccountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            AccountRepository = accountRepository;
        }

        public IEnumerable<Tuple<Account, Storage>> GetActiveAccounts(int userId)
        {
            return AccountRepository.GetAccounts(userId, (int)StorageStatus.Loaded);
        }
    }
}
