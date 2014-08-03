using System;
using Common.Logging;
using NHibernate;
using CloudBin.Core.Utilities;
using System.Diagnostics;
using NHibernate.Cfg;
using FluentNHibernate.Cfg;
using System.Reflection;
using System.Globalization;
using CloudBin.Core;
using System.Data;

namespace CloudBin.Data.NHibernate
{
    public sealed class SessionManager : IDatabaseSessionManager
    {
        private readonly ILog _log = LogManager.GetCurrentClassLogger();
        private const string SessionKey = "NHIBERNATE_CONTEXT_SESSION_KEY";
        private ISessionFactory _sessionFactory;

        IDatabaseSessionManager IDatabaseSessionManager.Initialize()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Configuration cfg = new Configuration();
            _sessionFactory = Fluently.Configure(cfg.Configure())
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildSessionFactory();
            stopWatch.Stop();
            _log.DebugFormat(CultureInfo.InvariantCulture, "Configured NHibernate in {0}ms", stopWatch.ElapsedMilliseconds);
            return this;
        }

        private ISession ContextSession
        {
            get { return RequestContext.Current.GetValue<ISession>(SessionKey); }
            set { RequestContext.Current.SetValue(SessionKey, value); }
        }

        public ISession CurrentSession
        {
            get { return ContextSession; }
        }

        void IDatabaseSessionManager.OpenSession()
        {
            OpenSession(null);
        }

        private void OpenSession(IInterceptor interceptor)
        {
            Verify.ThrowIfNotNull(() => ContextSession, () => new InvalidOperationException("Session already opened"));
            ISession session = interceptor != null ? _sessionFactory.OpenSession(interceptor) : _sessionFactory.OpenSession();
            ContextSession = session;
        }

        void IDatabaseSessionManager.CloseSession()
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
            {
                return;
            }

            if (session.IsOpen)
            {
                try
                {
                    if (session.Transaction != null)
                    {
                        if (session.Transaction.IsActive)
                        {
                            session.Transaction.Commit();
                        }
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

        void IDatabaseSessionManager.DoInTransaction(Action action)
        {
            ((IDatabaseSessionManager) this).DoInTransaction(action, IsolationLevel.Unspecified);
        }

        T IDatabaseSessionManager.DoInTransaction<T>(Func<T> action)
        {
            return ((IDatabaseSessionManager) this).DoInTransaction(action, IsolationLevel.Unspecified);
        }

        void IDatabaseSessionManager.DoInTransaction(Action action, IsolationLevel level)
        {
            Exception exception = null;
            ITransaction transaction = null;
            try
            {
                transaction = CurrentSession.BeginTransaction(level);
                action();
                transaction.Commit();
                return;
            }
            catch (Exception ex)
            {
                exception = ex;
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
            }
            throw exception;
        }

        T IDatabaseSessionManager.DoInTransaction<T>(Func<T> action, IsolationLevel level)
        {
            T result;
            Exception exception = null;
            ITransaction transaction = null;
            try
            {
                transaction = CurrentSession.BeginTransaction(level);
                result = action();
                transaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                exception = ex;
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
            }
            throw exception;
        }
    }
}
