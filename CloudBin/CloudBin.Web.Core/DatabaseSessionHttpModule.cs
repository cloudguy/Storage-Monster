using System;
using System.Web;
using CloudBin.Data;
using Common.Logging;

namespace CloudBin.Web.Core
{
    public sealed class DatabaseSessionHttpModule : IHttpModule
    {
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private static readonly Lazy<IOpenDatabaseConnectionPolicy> LazyOpenDatabaseConnectionPolicy = new Lazy<IOpenDatabaseConnectionPolicy>(() =>
        {
            return (IOpenDatabaseConnectionPolicy)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IOpenDatabaseConnectionPolicy));
        }, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private IOpenDatabaseConnectionPolicy OpenDatabaseConnectionPolicy
        {
            get { return LazyOpenDatabaseConnectionPolicy.Value; }
        }

        #region IHttpModule implementation

        void IHttpModule.Dispose()
        {
        }

        void IHttpModule.Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
            context.EndRequest += OnEndRequest;
        }

        #endregion

        private void OnBeginRequest(object sender, EventArgs e)
        {
            if (!OpenDatabaseConnectionPolicy.DatabaseConnectionRequired(HttpContext.Current))
            {
                return;
            }

            Logger.DebugFormat(System.Globalization.CultureInfo.InvariantCulture, "Opening session for request '{0}'", HttpContext.Current.Request.Path);

            DatabaseSessionManager.Current.OpenSession();
        }

        private void OnEndRequest(object sender, EventArgs e)
        {
            if (!OpenDatabaseConnectionPolicy.DatabaseConnectionRequired(HttpContext.Current))
            {
                return;
            }

            Logger.DebugFormat(System.Globalization.CultureInfo.InvariantCulture, "Closing session for request '{0}'", HttpContext.Current.Request.Path);

            DatabaseSessionManager.Current.CloseSession();
        }
    }
}
