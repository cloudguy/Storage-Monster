using System.Data;
using System;
using System.Data.Common;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace StorageMonster.Database
{
	public interface IConnectionProvider
	{
		DbConnection CreateConnection();
	    DbConnection CurrentConnection { get; }
	    void CloseCurrentConnection();
        void DoInTransaction(Action action, IsolationLevel level);
        T  DoInTransaction<T>(Func<T> action, IsolationLevel level);
        void DoInTransaction(Action action);
        T DoInTransaction<T>(Func<T> action);
	}
}
