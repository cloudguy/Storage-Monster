using System;
using System.Transactions;
using System.Globalization;
using System.Linq;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;

namespace StorageMonster.Database.MySql.Repositories
{
	public class StoragePluginsRepository : IStoragePluginsRepository
	{
	    protected IConnectionProvider ConnectionProvider { get; set; }
        protected const string TableName = "storage_plugins";
        protected const string SelectFieldList = "s.classpath AS ClassPath, s.id AS Id, s.status AS Status,  s.stamp AS Stamp";
        protected const string InsertFieldList = "(classpath, status) VALUES (@Classpath, @Status)";
       
        public static string GetSelectFieldList()
        {
            return SelectFieldList;
        }

        public StoragePluginsRepository(IConnectionProvider connectionprovider)
        {
            ConnectionProvider = connectionprovider;
        }

        public void SetStoragesStauses(int status)
        {
            SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "UPDATE {0} SET status=@Status", TableName);
                ConnectionProvider.CurrentConnection.Execute(query, new { Status = status });
            });
        }

        public StoragePlugin InitPluginStatus(string classPath, int status)
        {
            return SqlQueryExecutor.Execute(() =>
            {
                int? id;
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    string selectQuery = string.Format(CultureInfo.InvariantCulture, "SELECT id FROM {0} WHERE classpath = @ClassPath", TableName);
                    id = ConnectionProvider.CurrentConnection.Query<int?>(selectQuery, new {ClassPath = classPath}).FirstOrDefault();
                    if (id != null)
                    {
                        string updateQuery = string.Format(CultureInfo.InvariantCulture, "UPDATE {0}  SET status = @Status WHERE id = @Id", TableName);
                        ConnectionProvider.CurrentConnection.Execute(updateQuery, new {Status = status, Id = id.Value});
                    }
                    else
                    {
                        string insertQuery = string.Format(CultureInfo.InvariantCulture, "INSERT INTO {0} {1}; SELECT LAST_INSERT_ID();", TableName, InsertFieldList);
                        id = (int)ConnectionProvider.CurrentConnection.Query<long>(insertQuery, new { Status = status, ClassPath = classPath }).FirstOrDefault();
                        if (id.Value <= 0)
                            throw new MonsterDbException("Storage plugin insertion failed");
                    }

                    string fullSelectQuery = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} s WHERE id = @Id", SelectFieldList, TableName);
                    StoragePlugin storage = ConnectionProvider.CurrentConnection.Query<StoragePlugin>(fullSelectQuery, new { Id = id.Value}).FirstOrDefault();
                    transactionScope.Complete();
                    return storage;
                }
            });
        }

	}
}

