using System;
using System.Configuration;
using System.Globalization;
using System.Threading;

namespace StorageMonster.Database
{
    public abstract class DbConfigurationBase : IDbConfiguration
    {
        protected readonly string ConnectionKey;
        protected readonly Lazy<string> ConnectionStringInternal;

        protected DbConfigurationBase(string connectionKey)
        {
            ConnectionKey = connectionKey;
            ConnectionStringInternal = new Lazy<string>(GetConnectionString, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public String ConnectionString { get { return ConnectionStringInternal.Value; } }

        protected virtual string GetConnectionString()
        {
            ConnectionStringSettingsCollection connectionSettings = ConfigurationManager.ConnectionStrings;

            if (connectionSettings == null)
                throw new MonsterDbException("No connection settings found");

            ConnectionStringSettings connectionStringSettings = connectionSettings[ConnectionKey];

            if (connectionStringSettings == null)
                throw new MonsterDbException(string.Format(CultureInfo.InvariantCulture,
                                                           "Connection string with name {0} not found", ConnectionKey));
            return connectionStringSettings.ConnectionString;
        }
    }
}
