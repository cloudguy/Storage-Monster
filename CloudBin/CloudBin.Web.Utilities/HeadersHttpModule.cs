using System;
using System.Web;

namespace CloudBin.Web.Utilities
{
    public sealed class HeadersHttpModule : IHttpModule
    {
        private static readonly Lazy<IWebConfiguration> LazyWebConfiguration = new Lazy<IWebConfiguration>(() =>
        {
            return (IWebConfiguration) System.Web.Mvc.DependencyResolver.Current.GetService(typeof (IWebConfiguration));
        }, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private IWebConfiguration WebConfiguration
        {
            get { return LazyWebConfiguration.Value; }
        }

        void IHttpModule.Dispose()
        {
        }

        void IHttpModule.Init(HttpApplication application)
        {
            application.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication) sender;
            if (WebConfiguration.SendSecurityHeaders)
            {
                application.Response.AppendHeader(Constants.XssProtectionHeaderName, Constants.XssProtectionEnabledHeaderValue);
                application.Response.AppendHeader(Constants.FrameOptionsHeaderName, Constants.FrameOptionsDenyHeaderValue);
                application.Response.AppendHeader(Constants.ContentTypeOptionsHeaderName, Constants.ContentTypeOptionsNosniffHeaderValue);
            }
            if (WebConfiguration.RemoveVersionHeaders)
            {
                application.Response.Headers.Remove(Constants.PoweredByHeaderName);
                application.Response.Headers.Remove(Constants.AspNetVersionHeaderName);
                application.Response.Headers.Remove(Constants.AspNetMvcVersionHeaderName);
                application.Response.Headers.Remove(Constants.ServerHeaderName);
            }
        }
    }
}
