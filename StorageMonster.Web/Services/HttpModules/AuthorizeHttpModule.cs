using Common.Logging;
using StorageMonster.Services;
using StorageMonster.Web.Services.Security;
using System;
using System.Globalization;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.HttpModules
{
    public class AuthorizeHttpModule : IHttpModule
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AuthorizeHttpModule));

        public void Dispose()
        {
        }

        public void Init(HttpApplication application)
        {
            application.AuthorizeRequest += application_AuthorizeRequest;
            application.AcquireRequestState += application_AcquireRequestState;
            //application.PreSendRequestHeaders += application_PreSendRequestHeaders;
            application.EndRequest += application_EndRequest;
            application.ReleaseRequestState += application_ReleaseRequestState;
        }


        void application_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;

            if (!application.Response.IsClientConnected)
                return;
            HttpStatusCode code = (HttpStatusCode)application.Response.StatusCode;
            if (code == HttpStatusCode.Unauthorized)
            {
                AuthorizationHelper.RedirectToLogon(application.Context.Request, application.Response);
            }
        }


        void application_ReleaseRequestState(object sender, EventArgs e)
        {
#warning nh session could be closed here
            HttpApplication application = (HttpApplication)sender;
            //updating locale cookies
            var trackingService = DependencyResolver.Current.GetService<ITrackingService>();
#warning signature
            trackingService.SetLocaleTracking(application.Context);
        }


        void application_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpRequest request = application.Context.Request;

            Logger.DebugFormat(CultureInfo.InvariantCulture, "AuthorizeRequest {0}", request.AppRelativeCurrentExecutionFilePath);

            try
            {
                IAuthenticationService authenticationService = DependencyResolver.Current.GetService<IAuthenticationService>();
                authenticationService.AuthorizeRequest();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }
        }


        void application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            HttpRequest request = application.Context.Request;

            Logger.DebugFormat(CultureInfo.InvariantCulture, "AcquireRequestState {0}", request.AppRelativeCurrentExecutionFilePath);

            var localeProvider = DependencyResolver.Current.GetService<ILocaleProvider>();
            var trackingService = DependencyResolver.Current.GetService<ITrackingService>();

            Identity identity = context.User.Identity as Identity;
            string langName = string.Empty;


            if (identity == null || string.IsNullOrEmpty(identity.Locale))
            {
                //user not authenticated, first try is tracking cookie
                //if no cookie, use browser headers
                var trackingLocale = trackingService.GetTrackedLocaleName(context);
                if (trackingLocale != null)
                    langName = trackingLocale;
                else if (request.UserLanguages != null && request.UserLanguages.Length != 0)
                    langName = request.UserLanguages[0].Substring(0, 2);
            }
            else
            {
                langName = identity.Locale;
            }

            LocaleData locale = localeProvider.GetCultureByNameOrDefault(langName);
            localeProvider.SetThreadLocale(locale);
            if (identity.IsAuthenticated)
            {
                IAuthenticationService authenticationService = DependencyResolver.Current.GetService<IAuthenticationService>();
                authenticationService.SlideExpire(application.Context);
            }
        }
    }
}