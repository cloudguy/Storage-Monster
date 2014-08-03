using System;
using System.Data;

namespace CloudBin.Data
{
    public interface IDatabaseSessionManager
    {
        IDatabaseSessionManager Initialize();
        void DoInTransaction(Action action, IsolationLevel level);
        T DoInTransaction<T>(Func<T> action, IsolationLevel level);
        void DoInTransaction(Action action);
        T DoInTransaction<T>(Func<T> action);
        void OpenSession();
        void CloseSession();
    }
}
