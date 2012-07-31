using System.Configuration;
using System.Globalization;

namespace StorageMonster.Database.MySql
{
    public class DbConfiguration : IDbConfiguration
    {
        protected const string ConnectionKey = "StorageMonsterMySqlServices";

        public DbConfiguration()
        {
            ConnectionStringSettingsCollection connectionSettings = ConfigurationManager.ConnectionStrings;

            if (connectionSettings == null)
                throw new MonsterDbException("No connection settings found");

            ConnectionStringSettings connectionStringSettings = connectionSettings[ConnectionKey];

            if (connectionStringSettings == null)
                throw new MonsterDbException(string.Format(CultureInfo.InvariantCulture, "Connection string with name {0} not found", ConnectionKey));

            ConnectionString = connectionStringSettings.ConnectionString;
        }

        public string ConnectionString { get; private set; }
    }
}
