using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Data;
using StorageMonster.Utilities;

namespace StorageMonster.Database.MySql
{
    public class ConnectionProvider : IConnectionProvider
    {
        protected static Func<String, DbConnection> ConnectionBuilder { get; set; }
        protected static Func<String, DbCommand> CommandBuilder { get; set; }

        protected IDbConfiguration DbConfiguration { get; set; }

        private readonly object _locker = new object();

        protected static void Initialize()
        {
            //This is hack for Synology Nas server (ARMv6 proccessor)
            //Mono can't find MySql.Data.dll assembly, so i had to unlink it and load dynamically

            Assembly mysqlAssembly;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("StorageMonster.Database.MySql.Driver.MySql.Data.dll"))
            {
                if (stream == null)
                    throw new InvalidOperationException("Mysql driver not found in resources");

                Byte[] assemblyData = new Byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                mysqlAssembly = Assembly.Load(assemblyData);
            }

            Type mysqlConnectionType = mysqlAssembly.GetType("MySql.Data.MySqlClient.MySqlConnection");
            Type mysqlCommandType = mysqlAssembly.GetType("MySql.Data.MySqlClient.MySqlCommand");

            // Constructor obtained dynamically
            // Because of issues of using mysql connector on mono platform 2.10
            // with ARMv6 proccessor architectire (mysql driver does not want to be loaded either from gac or from bin folder)
            ConstructorInfo mysqlConnectionConstructor = mysqlConnectionType.GetConstructor(new[] { typeof(string) });
            ConstructorInfo mysqlCommandConstructor = mysqlCommandType.GetConstructor(new[] { typeof(string) });


            var paramConnectionString = Expression.Parameter(typeof(string), "connectionString");

            ConnectionBuilder = Expression.Lambda<Func<String, DbConnection>>(
                Expression.New(mysqlConnectionConstructor, paramConnectionString), new[] { paramConnectionString }
                ).Compile();


            var paramQuery = Expression.Parameter(typeof(string), "query");
            CommandBuilder = Expression.Lambda<Func<String, DbCommand>>(
                Expression.New(mysqlCommandConstructor, paramQuery), new[] { paramQuery }
                ).Compile();
        }

        public ConnectionProvider(IDbConfiguration dbConfiguration)
        {
            DbConfiguration = dbConfiguration;
        }

        public IDbConnection CreateConnection()
        {
            if (ConnectionBuilder == null)
            {
                lock (_locker)
                {
                    if (ConnectionBuilder == null)
                        Initialize();
                }
            }

// ReSharper disable PossibleNullReferenceException
            DbConnection connection = ConnectionBuilder(DbConfiguration.ConnectionString);
// ReSharper restore PossibleNullReferenceException
            connection.Open();
            DbCommand command = CommandBuilder("SET time_zone='+00:00'; SET names 'utf8';");
            command.Connection = connection;
            command.ExecuteNonQuery();
            return connection;
        }

        public void CloseCurrentConnection()
        {
            IDbConnection connection = RequestContext.DbConnection;
            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();
            RequestContext.DbConnection = null;
        }

        public IDbConnection CurrentConnection
        {
            get
            {
                IDbConnection connection = RequestContext.DbConnection;
                if (connection == null || connection.State == ConnectionState.Closed)
                {
                    connection = CreateConnection();
                    RequestContext.DbConnection = connection;
                }
                return connection;
            }
        }
    }
}
