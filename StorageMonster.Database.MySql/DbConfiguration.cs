using System.Configuration;
using System.Globalization;

namespace StorageMonster.Database.MySql
{
    public class DbConfiguration : IDbConfiguration
    {
        private const string ConnectionKey = "StorageMonsterMySqlServices";
        private static string _connectionString;
        private static object _locker = new object();

        public string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    lock (_locker)
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
