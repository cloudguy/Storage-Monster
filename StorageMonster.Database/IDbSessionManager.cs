using System;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace StorageMonster.Database
{
    public interface IDbSessionManager
    {
        void Init();
        void DoInTransaction(Action action, IsolationLevel level);
        T  DoInTransaction<T>(Func<T> action, IsolationLevel level);
        void DoInTransaction(Action action);
        T DoInTransaction<T>(Func<T> action);
    }
}
