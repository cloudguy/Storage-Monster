using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Domain;
using System.Globalization;
using System.Transactions;
using StorageMonster.Database.Repositories;

namespace StorageMonster.Database.PgSql.Repositories
{
    public class StoragePluginsRepository : IStoragePluginsRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private const string TableName = "storage_plugins";
        private const string SelectFieldList = "s.classpath AS ClassPath, s.id AS Id, s.status AS Status,  s.stamp AS Stamp";
        private const string InsertFieldList = "(classpath, status) VALUES (@ClassPath, @Status)";

        public static string GetSelectFieldList()
        {
            return SelectFieldList;
        }

        public StoragePluginsRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public void SetStoragesStatuses(int status)
        {
            SqlQueryExecutor.Execute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "UPDATE {0} SET status=@Status", TableName);
                _connectionProvider.CurrentConnection.Execute(query, new { Status = status });
            });
        }

        public StoragePlugin InitPluginStatus(string classPath, int status)
        {
            if (string.IsNullOrEmpty(classPath))
                throw new ArgumentNullException(classPath);

            return SqlQueryExecutor.Execute(() =>
            {                
                return _connectionProvider.DoInTransaction(() =>
                {
                    int? id;
                    string selectQuery = string.Format(CultureInfo.InvariantCulture, "SELECT id FROM {0} WHERE classpath = @ClassPath", TableName);
                    id = _connectionProvider.CurrentConnection.Query<int?>(selectQuery, new { ClassPath = classPath }).FirstOrDefault();
                    if (id != null)
                    {
                        string updateQuery = string.Format(CultureInfo.InvariantCulture, "UPDATE {0}  SET status = @Status WHERE id = @Id", TableName);
                        _connectionProvider.CurrentConnection.Execute(updateQuery, new { Status = status, Id = id.Value });
                    }
                    else
                    {
                        string insertQuery = string.Format(CultureInfo.InvariantCulture, "INSERT INTO {0} {1} RETURNING id;", TableName, InsertFieldList);
                        id = _connectionProvider.CurrentConnection.Query<int>(insertQuery, new { Status = status, ClassPath = classPath }).FirstOrDefault();
                        if (id.Value <= 0)
                            throw new MonsterDbException("Storage plugin insertion failed");
                    }

                    string fullSelectQuery = string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} s WHERE id = @Id", SelectFieldList, TableName);
                    StoragePlugin storage = _connectionProvider.CurrentConnection.Query<StoragePlugin>(fullSelectQuery, new { Id = id.Value }).FirstOrDefault();

                    return storage;
                });
            });
        }
    }
}
