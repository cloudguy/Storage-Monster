using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;

namespace StorageMonster.Database.MySql.Repositories
{
    public class StorageAccountRepository : IStorageAccountRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private const string SelectFieldList = "a.id AS Id, a.user_id AS UserId, a.storage_plugin_id AS StoragePluginId, a.account_name AS AccountName, a.stamp AS Stamp";
        private const string InsertFieldList = "(user_id, storage_plugin_id, account_name) VALUES(@UserId, @StoragePluginId, @AccountName)";
        private const string TableName = "storage_accounts";

        public StorageAccountRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public IEnumerable<Tuple<StorageAccount, StoragePlugin>> GetAccounts(int userId, int storageStatus)
        {
            return SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0}, {1} FROM {2} a INNER JOIN storage_plugins s ON a.storage_plugin_id=s.id WHERE a.user_id=@UserId AND s.status = @StorageStatus;", SelectFieldList, StoragePluginsRepository.GetSelectFieldList(), TableName);

                return _connectionProvider.CurrentConnection.Query<StorageAccount, StoragePlugin, Tuple<StorageAccount, StoragePlugin>>(query, 
                    (a, s) => new Tuple<StorageAccount, StoragePlugin>(a, s),
                    new { UserId = userId, StorageStatus = storageStatus }, 
                    null, 
                    false,
                    "ClassPath", 
                    null, 
                    CommandType.Text);
                    
            });
        }

        public StorageAccount Load(int id)
        {
            return SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} a WHERE id = @Id;", SelectFieldList, TableName);
                return _connectionProvider.CurrentConnection.Query<StorageAccount>(query, new {Id = id}).FirstOrDefault();
            });
        }
        public StorageAccount Load(string accountName, int storagePluginId, int userId)
        {
            return SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} a WHERE a.storage_plugin_id = @StoragePluginId AND a.user_id = @UserId AND a.account_name = @AccountName;", SelectFieldList, TableName);
                return _connectionProvider.CurrentConnection.Query<StorageAccount>(query, new { StoragePluginId = storagePluginId, UserId = userId, AccountName = accountName }).FirstOrDefault();
            });
        }

        public void Delete(int storageAccountId)
        {
            SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "DELETE FROM {0} WHERE id = @Id;", TableName);
                _connectionProvider.CurrentConnection.Execute(query, new { Id = storageAccountId });
            });
        }

        public StorageAccount Insert(StorageAccount account)
        {
            if (account == null)
                throw new ArgumentNullException("account");

            return SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "INSERT INTO {1} {0}; SELECT LAST_INSERT_ID();", InsertFieldList, TableName);
                int insertedId = (int)_connectionProvider.CurrentConnection.Query<long>(query, new { account.AccountName, account.StoragePluginId, account.UserId, account.Stamp }).FirstOrDefault();
                if (insertedId <= 0)
                    throw new MonsterDbException("Account insertion failed");

                String idAndStampQuery = string.Format(CultureInfo.InvariantCulture, "SELECT id AS Id, stamp AS Stamp FROM {0} WHERE id=@Id;", TableName);
                IdAndStamp idAndStamp = _connectionProvider.CurrentConnection.Query<IdAndStamp>(idAndStampQuery, new { Id = insertedId }).FirstOrDefault();

                if (idAndStamp == null)
                    throw new MonsterDbException("Account insertion failed");

                account.Id = idAndStamp.Id;
                account.Stamp = idAndStamp.Stamp;
                return account;
            });
        }
    }
}
