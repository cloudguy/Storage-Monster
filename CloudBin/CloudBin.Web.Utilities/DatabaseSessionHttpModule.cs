using System;
using System.Web;
using Common.Logging;
using CloudBin.Data;

namespace CloudBin.Web.Utilities
{
    public sealed class DatabaseSessionHttpModule : IHttpModule
    {
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private static readonly Lazy<IWebConfiguration> LazyWebConfiguration = new Lazy<IWebConfiguration>(() =>
        {
            return (IWebConfiguration) System.Web.Mvc.DependencyResolver.Current.GetService(typeof (IWebConfiguration));
        }, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private IWebConfiguration WebConfiguration
        {
            get { return LazyWebConfiguration.Value; }
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
            if (WebConfiguration.DoNotOpenDbSessionForScriptAndContent && HttpContext.Current.Request.IsScriptOrContentRequest())
            {
                return;
            }

            Logger.DebugFormat(System.Globalization.CultureInfo.InvariantCulture, "Opening session for request '{0}'", HttpContext.Current.Request.Path);

            DatabaseSessionManager.Current.OpenSession();
        }

        private void OnEndRequest(object sender, EventArgs e)
        {
            if (WebConfiguration.DoNotOpenDbSessionForScriptAndContent && HttpContext.Current.Request.IsScriptOrContentRequest())
            {
                return;
            }

            Logger.DebugFormat(System.Globalization.CultureInfo.InvariantCulture, "Closing session for request '{0}'", HttpContext.Current.Request.Path);

            DatabaseSessionManager.Current.CloseSession();
        }
    }
}
