using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StorageMonster.DB.Repositories;
using StorageMonster.DB.Domain;

namespace StorageMonster.DB.MySQL.Repositories
{
	public class StorageRepository : IStorageRepository
	{
		protected IConnectionProvider Connectionprovider;
        protected const string TableName = "storages";
        protected const string SelectFieldList = "s.classpath AS ClassPath, s.id AS Id, s.status AS Status,  s.stamp AS Stamp";
       
        public static string GetSelectFieldList()
        {
            return SelectFieldList;
        }

        public StorageRepository(IConnectionProvider connectionprovider)
        {
            Connectionprovider = connectionprovider;
        }

        public void SetStoragesStauses(int status)
        {
            SqlQueryExecutor.Exceute(() =>
            {
                String query = string.Format(CultureInfo.InvariantCulture, "UPDATE {0} SET status=@Status", TableName);
                Connectionprovider.CurrentConnection.Execute(query, new { Status = status });
            });
        }

        public int InitPluginStatus(string classPath, int status)
        {
            return SqlQueryExecutor.Exceute(() =>
            {
                const string query = "select init_plugin_status(@ClassPath, @Status);";
                return Connectionprovider.CurrentConnection.Query<int>(query, new {ClassPath = classPath, Status = status}).FirstOrDefault();
            });
        }

	}
}

