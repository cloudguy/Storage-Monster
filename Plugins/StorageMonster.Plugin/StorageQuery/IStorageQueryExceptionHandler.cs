using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Plugin.StorageQuery
{
    public interface IStorageQueryExceptionHandler
    {
        IStorageQueryExecutor CatchIt(Action<Exception> exceptionHandler);        
    }
}
