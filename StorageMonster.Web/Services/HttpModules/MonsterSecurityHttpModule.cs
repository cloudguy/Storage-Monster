using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageMonster.Web.Services.HttpModules
{
    public class MonsterSecurityHttpModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication application)
        {
            application.PreSendRequestHeaders += new EventHandler(application_PreSendRequestHeaders);
        }

        void application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            application.Response.AddHeader("X-XSS-Protection", "1; mode=block");
            application.Response.AddHeader("X-Frame-Options", "deny");
            application.Response.AddHeader("X-Content-Type-Options", "nosniff");           
        }
    }
}