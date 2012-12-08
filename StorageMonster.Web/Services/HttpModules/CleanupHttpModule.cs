using StorageMonster.Database;
using System;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.HttpModules
{
    public class CleanupHttpModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.EndRequest += application_EndRequest;
        }

        void application_EndRequest(object sender, EventArgs e)
        {
            IConnectionProvider connectionProvider = DependencyResolver.Current.GetService<IConnectionProvider>();
            connectionProvider.CloseCurrentConnection();
        }
    }
}