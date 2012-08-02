using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace StorageMonster.Web.Services.HttpModules
{
    public class MonsterLowercaseRoutesHttpModule : IHttpModule
    {
        public void Dispose()
		{
		}

		public void Init(HttpApplication context)
		{
            if (context == null)
                throw new ArgumentNullException("context");

            context.BeginRequest +=new EventHandler(context_BeginRequest);
		}

        void  context_BeginRequest(object sender, EventArgs e)
        {            
            HttpApplication application = (HttpApplication)sender;
            HttpRequest request = application.Request;
            string lowercaseURL = (request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.AbsolutePath);
            if (Regex.IsMatch(lowercaseURL, @"[A-Z]"))
            {
                lowercaseURL = lowercaseURL.ToLower() + HttpContext.Current.Request.Url.Query;

                application.Response.Clear();
                application.Response.Status = "301 Moved Permanently";
                application.Response.AddHeader("Location", lowercaseURL);
                application.Response.End();
            }
        }
    }
}

