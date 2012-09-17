using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;
using StorageMonster.Database;

namespace StorageMonster.Database.PgSql
{
    public class DbConfiguration : IDbConfiguration
    {
        private const string ConnectionKey = "StorageMonsterPgSqlServices";
        private static string _connectionString;
        private static readonly object Locker = new object();

        public string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    lock (Locker)
                    {
                        if (_connectionString == null)
                        {
                            ConnectionStringSettingsCollection connectionSettings = ConfigurationManager.ConnectionStrings;

                            if (connectionSettings == null)
                                throw new MonsterDbException("No connection settings found");

                            ConnectionStringSettings connectionStringSettings = connectionSettings[ConnectionKey];

                            if (connectionStringSettings == null)
                                throw new MonsterDbException(string.Format(CultureInfo.InvariantCulture, "Connection string with name {0} not found", ConnectionKey));

                            _connectionString = connectionStringSettings.ConnectionString;
                        }
                    }
                }

                return _connectionString;
            }
        }
    }
}

