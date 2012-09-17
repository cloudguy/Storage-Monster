using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Data;
using StorageMonster.Utilities;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace StorageMonster.Database.MySql
{
    public class ConnectionProvider : IConnectionProvider
    {
        private static Func<String, DbConnection> _connectionBuilder;
        private static Func<String, DbCommand> _commandBuilder;

        private readonly IDbConfiguration _dbConfiguration;

        private readonly object _locker = new object();

        private static void Initialize()
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

            _connectionBuilder = Expression.Lambda<Func<String, DbConnection>>(
                Expression.New(mysqlConnectionConstructor, paramConnectionString), new[] { paramConnectionString }
                ).Compile();


            var paramQuery = Expression.Parameter(typeof(string), "query");
            _commandBuilder = Expression.Lambda<Func<String, DbCommand>>(
                Expression.New(mysqlCommandConstructor, paramQuery), new[] { paramQuery }
                ).Compile();
        }

        public ConnectionProvider(IDbConfiguration dbConfiguration)
        {
            _dbConfiguration = dbConfiguration;
        }

        public DbConnection CreateConnection()
        {
            if (_connectionBuilder == null)
            {
                lock (_locker)
                {
                    if (_connectionBuilder == null)
                        Initialize();
                }
            }

// ReSharper disable PossibleNullReferenceException
            DbConnection connection = _connectionBuilder(_dbConfiguration.ConnectionString);
// ReSharper restore PossibleNullReferenceException
            connection.Open();
            using (DbCommand command = _commandBuilder("SET time_zone='+00:00'; SET names 'utf8';"))
            {
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
            return connection;
        }

        public void CloseCurrentConnection()
        {
            IDbConnection connection = RequestContext.DbConnection;
            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();
            RequestContext.DbConnection = null;
        }

        public DbConnection CurrentConnection
        {
            get
            {
                DbConnection connection = RequestContext.DbConnection;
                if (connection == null || connection.State == ConnectionState.Closed)
                {
                    connection = CreateConnection();
                    RequestContext.DbConnection = connection;
                }
                return connection;
            }
        }

        public void DoInTransaction(Action action, IsolationLevel level)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = level }))
            {
                CurrentConnection.EnlistTransaction(Transaction.Current);
                action();
                scope.Complete();
            }
        }

        public T DoInTransaction<T>(Func<T> action, IsolationLevel level)
        {
            if (action == null)
                throw new ArgumentNullException("action");
           
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = level }))
            {
                CurrentConnection.EnlistTransaction(Transaction.Current);
                T result = action();
                scope.Complete();
                return result;
            }
        }

        public void DoInTransaction(Action action)
        {
            DoInTransaction(action, IsolationLevel.ReadCommitted);
        }

        public T DoInTransaction<T>(Func<T> action)
        {
            return DoInTransaction<T>(action, IsolationLevel.ReadCommitted);
        }
    }
}
