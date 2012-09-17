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
        protected Func<Exception, Exception> ExceptionRethrowHandler;

        public bool CanHandle(Type exceptionType)
        {
            if (exceptionType != ExceptionType)
                return false;

            return ExceptionHandler != null || ExceptionRethrowHandler != null;
        }

        internal StorageQueryExceptionHandler(IStorageQueryExecutor parentExecutor, Type exceptionType)
        {
            ParentExecutor = parentExecutor;
            ExceptionType = exceptionType;
        }

        public IStorageQueryExecutor CatchIt(Action<Exception> exceptionHandler)
        {
            if (exceptionHandler == null)
                throw new ArgumentNullException("exceptionHandler");

            if (ExceptionRethrowHandler != null)
                throw new InvalidOperationException("Exception handler already defined");

            ExceptionHandler = exceptionHandler;
            return ParentExecutor;
        }

        public bool HandleException(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            if (ExceptionType != exception.GetType())
                return false;

            if (ExceptionHandler != null)
            {
                ExceptionHandler(exception);
                return true;
            }

            if (ExceptionRethrowHandler != null)
            {
                throw ExceptionRethrowHandler(exception);
            }
            
            return false;
        }

        public IStorageQueryExecutor Throw(Func<Exception, Exception> exceptionRethrowHandler)
        {
            if (exceptionRethrowHandler == null)
                throw new ArgumentNullException("exceptionRethrowHandler");

            if (ExceptionHandler != null)
                throw new InvalidOperationException("Exception handler already defined");

            ExceptionRethrowHandler = exceptionRethrowHandler;
            return ParentExecutor;
        }
    }
}
