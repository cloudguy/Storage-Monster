using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Plugin.StorageQuery
{
    public interface IStorageQueryExecutor
    {
        IStorageQueryExceptionHandler IfExceptionIs(Type exceptiontype);
        T Run<T>() where T : StorageQueryResult;
    }
}
