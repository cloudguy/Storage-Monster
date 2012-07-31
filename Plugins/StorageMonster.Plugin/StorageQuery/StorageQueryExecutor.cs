using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Plugin.StorageQuery
{
    public class StorageQueryExecutor : IStorageQueryExecutor
    {
        protected Action ActionToPerform;

        private IList<StorageQueryExceptionHandler> _exceptionHandlers = new List<StorageQueryExceptionHandler>();

        protected StorageQueryExecutor(Action actionToPerform)
        {
            ActionToPerform = actionToPerform;
        }

        public static StorageQueryExecutor WithAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return new StorageQueryExecutor(action);
        }

        public IStorageQueryExceptionHandler IfExceptionIs(Type exceptiontype)
        {
            if (exceptiontype == null)
                throw new ArgumentNullException("exceptiontype");

            var handler = new StorageQueryExceptionHandler(this, exceptiontype);
            _exceptionHandlers.Add(handler);
            return handler;
        }

        public void Run()
        {
            Exception error = null;            
            try
            {
                ActionToPerform();
            }
            catch (Exception exception)
            {
                error = exception;
            }
            if (error != null)
            {
                Type exceptionType = error.GetType();
                bool exceptionHandled = false;
                foreach (StorageQueryExceptionHandler handler in _exceptionHandlers)
                {
                    if (handler.HandleException(error))
                    {
                        exceptionHandled = true;
                        break;
                    }
                }
            
                if (!exceptionHandled)
                    throw error;
            }            
        }
    }
}
