using System;
using System.Web;

namespace StorageMonster.Web.Services.HttpModules
{
    public class SecurityHttpModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication application)
        {
            application.PreSendRequestHeaders += application_PreSendRequestHeaders;
        }

        private void application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication) sender;
            application.Response.AddHeader("X-XSS-Protection", "1; mode=block");
            application.Response.AddHeader("X-Frame-Options", "deny");
            application.Response.AddHeader("X-Content-Type-Options", "nosniff");
        }
    }
}