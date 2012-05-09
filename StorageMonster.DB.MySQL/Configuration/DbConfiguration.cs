using System.Configuration;
using System.Globalization;
using StorageMonster.DB.Configuration;

namespace StorageMonster.DB.MySQL.Configuration
{
    public class DbConfiguration : IDbConfiguration
    {
        protected const string ConnectionKey = "StorageMonsterMySqlServices";

        public DbConfiguration()
        {
            ConnectionStringSettingsCollection connectionSettings = ConfigurationManager.ConnectionStrings;

            if (connectionSettings == null)
                throw new StorageMonsterDbException("No connection settings found");

            ConnectionStringSettings connectionStringSettings = connectionSettings[ConnectionKey];

            if (connectionStringSettings == null)
                throw new StorageMonsterDbException(string.Format(CultureInfo.InvariantCulture, "Connection string with name {0} not found", ConnectionKey));

            ConnectionString = connectionStringSettings.ConnectionString;
        }

        public string ConnectionString { get; private set; }
    }
}
