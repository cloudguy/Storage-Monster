using StorageMonster.Utilities;
using System;
using System.Data;
using System.Data.Common;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace StorageMonster.Database
{
    public abstract class ConnectionProviderBase : IConnectionProvider
    {
        protected readonly IDbConfiguration DbConfiguration;

        protected ConnectionProviderBase(IDbConfiguration dbConfiguration)
        {
            DbConfiguration = dbConfiguration;
        }

        public abstract DbConnection CreateConnection();

        public virtual DbConnection CurrentConnection
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

        public virtual void CloseCurrentConnection()
        {
            IDbConnection connection = RequestContext.DbConnection;
            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();
            RequestContext.DbConnection = null;
        }

        public virtual void DoInTransaction(Action action, IsolationLevel level)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = level }))
            {
                CurrentConnection.EnlistTransaction(Transaction.Current);
                action();
                scope.Complete();
            }
        }

        public virtual T DoInTransaction<T>(Func<T> action, IsolationLevel level)
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

        public virtual void DoInTransaction(Action action)
        {
            DoInTransaction(action, IsolationLevel.ReadCommitted);
        }

        public virtual T DoInTransaction<T>(Func<T> action)
        {
            return DoInTransaction(action, IsolationLevel.ReadCommitted);
        }
    }
}
