using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using StorageMonster.DB.Domain;
using StorageMonster.DB.Repositories;
using StorageMonster.Util;

namespace StorageMonster.DB.MySQL.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        protected IConnectionProvider Connectionprovider;
        protected string SelectFieldList = "a.id AS Id, user_id AS UserId, a.storage_id AS StorageId, a.account_server AS AccountServer, a.account_login AS AccountLogin, a.stamp AS Stamp";
        protected const string TableName = "accounts";

        public AccountRepository(IConnectionProvider connectionprovider)
        {
            Connectionprovider = connectionprovider;
        }

        public IEnumerable<Tuple<Account, Storage>> GetAccounts(int userId, int storageStatus)
        {
            return SqlQueryExecutor.Exceute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0}, {1} FROM {2} a INNER JOIN storages s ON a.storage_id=s.id WHERE a.user_id=@UserId AND s.status = @StorageStatus;", SelectFieldList, StorageRepository.GetSelectFieldList(), TableName);

                return Connectionprovider.CurrentConnection.Query<Account, Storage, Tuple<Account, Storage>>(query, 
                    (a, s) => new Tuple<Account, Storage>(a, s),
                    new { UserId = userId, StorageStatus = storageStatus }, 
                    null, 
                    false,
                    "ClassPath", 
                    null, 
                    CommandType.Text);
                    
            });
        }
    }
}
