using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Plugin.StorageQuery
{
    internal class StorageQueryExceptionHandler : IStorageQueryExceptionHandler
    {
        protected Type ExceptionType;
        protected IStorageQueryExecutor ParentExecutor;
        protected Action<Exception> ExceptionHandler;

        internal StorageQueryExceptionHandler(IStorageQueryExecutor parentExecutor, Type exceptionType)
        {
            ParentExecutor = parentExecutor;
            ExceptionType = exceptionType;
        }

        public IStorageQueryExecutor CatchIt(Action<Exception> exceptionHandler)
        {
            if (exceptionHandler == null)
                throw new ArgumentNullException("exceptionHandler");

            ExceptionHandler = exceptionHandler;
            return ParentExecutor;
        }

        public bool HandleException(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            if (ExceptionType == exception.GetType())
            {
                ExceptionHandler(exception);
                return true;
            }
            return false;
        }
    }
}
