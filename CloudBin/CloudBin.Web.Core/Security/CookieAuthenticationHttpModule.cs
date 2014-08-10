using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using CloudBin.Web.Core.Configuration;

namespace CloudBin.Web.Core.Security
{
    public sealed class CookieAuthenticationHttpModule : IHttpModule
    {
        private static readonly Lazy<IAuthenticationConfiguration> AuthConfigSection = new Lazy<IAuthenticationConfiguration>(() =>
        {
            return (IAuthenticationConfiguration)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IAuthenticationConfiguration));
        }, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private static readonly Lazy<IAuthenticationService> AuthenticationService = new Lazy<IAuthenticationService>(() =>
        {
            return (IAuthenticationService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IAuthenticationService));
        }, System.Threading.LazyThreadSafetyMode.PublicationOnly);


        void IHttpModule.Init(HttpApplication application)
        {
            application.AuthenticateRequest += OnAuthenticateRequest;
            application.EndRequest += OnEndRequest;
        }

        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            AuthenticationService.Value.AuthenticateRequest(application.Context);
        }

        private void OnEndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            var context = application.Context;
            var response = context.Response;
            var request = context.Request;
            if (!response.IsClientConnected)
            {
                return;
            }

            var authCookieName = AuthenticationService.Value.AuthCookieName;

            if (response.Cookies.Keys.Cast<string>().Contains(authCookieName))
            {
                response.Cache.SetCacheability(HttpCacheability.NoCache, "Set-Cookie");
            }

            HttpStatusCode code = (HttpStatusCode)application.Response.StatusCode;

#warning ajax redirect

            if (code == HttpStatusCode.Unauthorized)
            {
                var signInUrl = AuthConfigSection.Value.SignInUrl;
                if (request.IsAjaxRequest() || !request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    application.Response.Redirect(signInUrl, false);
                    return;
                }
                string returnUrl = Uri.EscapeUriString(request.QueryString.AllKeys.Contains("ReturnUrl") 
                    ? request.QueryString["ReturnUrl"] 
                    : application.Context.Request.Url.PathAndQuery);

                if (string.IsNullOrEmpty(returnUrl) || returnUrl.Equals("/", StringComparison.OrdinalIgnoreCase))
                {
                    application.Response.Redirect(signInUrl, false);
                }
                else
                {
                    var delimiter = signInUrl.Contains("?") ? "&" : "?";
                    application.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", signInUrl, delimiter, returnUrl), false);
                }
            }
        }

        void IHttpModule.Dispose()
        {
        }
    }
}
