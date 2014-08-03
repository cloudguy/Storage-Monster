using System;
using System.Web;

namespace CloudBin.Web.Utilities
{
    public sealed class SecurityHeadersHttpModule : IHttpModule
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
            if (!WebConfiguration.SendSecurityHeaders)
            {
                return;
            }
            HttpApplication application = (HttpApplication) sender;
            application.Response.Headers[Constants.XssProtectionHeaderName] = Constants.XssProtectionEnabledHeaderValue;
            application.Response.Headers[Constants.FrameOptionsHeaderName] = Constants.FrameOptionsDenyHeaderValue;
            application.Response.Headers[Constants.ContentTypeOptionsHeaderName] = Constants.ContentTypeOptionsNosniffHeaderValue;
        }
    }
}
