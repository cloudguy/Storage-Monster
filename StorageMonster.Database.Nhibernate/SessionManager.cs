using Common.Logging;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using StorageMonster.Utilities;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace StorageMonster.Database.Nhibernate
{
    public class SessionManager : IDbSessionManager
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(SessionManager));
        private const string SessionKey = "NHIBERNATE_CONTEXT_SESSION";
        private ISessionFactory _sessionFactory;

        public static SessionManager Instance { get; private set; }

        public IDbSessionManager Init()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var cfg = new Configuration();
            _sessionFactory = Fluently.Configure(cfg.Configure())
                .Mappings(m =>m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildSessionFactory();
            stopWatch.Stop();
            _log.TraceFormat(CultureInfo.InvariantCulture, "Configured NHibernate in {0}ms", stopWatch.ElapsedMilliseconds);
            Instance = this;
            return this;
        }

        private ISession ContextSession
        {
            get { return RequestContext.GetValue<ISession>(SessionKey); }
            set { RequestContext.SetValue(SessionKey, value); }
        }

        public static ISession CurrentSession
        {
            get { return Instance.ContextSession; }
        }

        public void OpenSession()
        {
            OpenSession(null);
        }

        private void OpenSession(IInterceptor interceptor)
        {
            if (ContextSession != null)
                throw new InvalidOperationException("Session already opened");
            ISession session  = interceptor != null ? _sessionFactory.OpenSession(interceptor) : _sessionFactory.OpenSession();
            ContextSession = session;
        }

        public void CloseSession()
        {
            try
            {
                CloseSession(ContextSession);
            }
            finally
            {
                ContextSession = null;
            }
        }

        private void CloseSession(ISession session)
        {
            if (session == null)
                return;

            if (session.IsOpen)
            {
                try
                {
                    if (session.Transaction != null)
                    {
                        if (session.Transaction.IsActive)
                            session.Transaction.Commit();
                    }
                    session.Flush();
                }
                finally
                {
                    session.Close();
                    session.Dispose();
                }
            }
        }

        public void DoInTransaction(Action action, IsolationLevel level)
        {
            throw new NotImplementedException();
        }

        public T DoInTransaction<T>(Func<T> action, IsolationLevel level)
        {
            throw new NotImplementedException();
        }

        public void DoInTransaction(Action action)
        {
            throw new NotImplementedException();
        }

        public T DoInTransaction<T>(Func<T> action)
        {
            throw new NotImplementedException();
        }
       

      /*
        private ITransaction ContextTransaction
        {            
            get
            {
                return (ITransaction)GlobalContext.Get(TRANSACTION_KEY);               
            }            
            set
            {
                GlobalContext.Set(TRANSACTION_KEY, value);                
            }
        }

        /// <summary>
        /// Flushes anything left in the session and closes the connection.
        /// </summary>
        public void CloseSession()
        {
            ISession session = ContextSession;

            if (session != null && session.IsOpen)
            {
                session.Flush();
                session.Close();
            }

            ContextSession = null;
        }

        /// <summary>
        /// Determines whether [has open transaction].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has open transaction]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasOpenTransaction()
        {
            ITransaction transaction = ContextTransaction;

            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            ITransaction transaction = ContextTransaction;

            try
            {
                if (HasOpenTransaction())
                {
                    transaction.Rollback();
                }

                ContextTransaction = null;
            }
            finally
            {
                CloseSession();
            }
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            ITransaction transaction = ContextTransaction;

            if (transaction == null)
            {
                transaction = GetSession().BeginTransaction();
                ContextTransaction = transaction;
            }
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            ITransaction transaction = ContextTransaction;

            try
            {
                if (HasOpenTransaction())
                {
                    transaction.Commit();
                    ContextTransaction = null;
                }
            }
            catch (HibernateException)
            {
                RollbackTransaction();
                throw;
            }
        }*/

    }
}
