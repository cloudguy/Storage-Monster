using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;
using StorageMonster.Utilities;
using System.Data.Common;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace StorageMonster.Database.PgSql
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly IDbConfiguration _dbConfiguration;

        public ConnectionProvider(IDbConfiguration dbConfiguration)
        {
            _dbConfiguration = dbConfiguration;
        }

        public DbConnection CreateConnection()
        {  
            DbConnection connection = new NpgsqlConnection(_dbConfiguration.ConnectionString);
           
            connection.Open();           
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SET TIME ZONE 'UTC';";                
                command.ExecuteNonQuery();
                command.CommandText = "SET CLIENT_ENCODING TO 'UTF8';";
                command.ExecuteNonQuery();
            }
            return connection;
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

        public void CloseCurrentConnection()
        {
            IDbConnection connection = RequestContext.DbConnection;
            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();
            RequestContext.DbConnection = null;
        }

        public void DoInTransaction(Action action, IsolationLevel level)
        {
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