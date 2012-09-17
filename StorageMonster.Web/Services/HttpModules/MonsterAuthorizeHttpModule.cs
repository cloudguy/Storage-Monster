using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Common.Logging;
using StorageMonster.Services;
using System.Globalization;
using System.Threading;
using StorageMonster.Utilities;
using StorageMonster.Web.Services.Configuration;
using StorageMonster.Web.Services.Security;



namespace StorageMonster.Web.Services.HttpModules
{
	public class MonsterAuthorizeHttpModule : IHttpModule
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(MonsterAuthorizeHttpModule));
		
		public void Dispose()
		{
		}
		
		public void Init(HttpApplication application)
		{
			application.AuthorizeRequest += application_AuthorizeRequest;
			application.AcquireRequestState += application_AcquireRequestState;
            application.PreSendRequestHeaders += application_PreSendRequestHeaders;
            application.EndRequest += application_EndRequest;
		}

       
        void application_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;            
            
            //updating locale cookies
            IWebConfiguration webConfiguration = IocContainer.Instance.Resolve<IWebConfiguration>();
            var trackingService = IocContainer.Instance.Resolve<ITrackingService>();
            trackingService.SetLocaleTracking(application.Context);
           
            HttpStatusCode code = (HttpStatusCode)application.Response.StatusCode;
            if (code == HttpStatusCode.Unauthorized)
            {                
                if (!application.Context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    application.Response.Redirect("~/account/logon");
                    return;
                }
                string returnUrl = Uri.EscapeUriString(application.Context.Request.Url.PathAndQuery); 
                if (returnUrl.Equals("/", StringComparison.OrdinalIgnoreCase))
                    application.Response.Redirect("~/account/logon");
                else
                    application.Response.Redirect("~/account/logon?returnUrl=" + returnUrl);
            }
        }
        

        void application_PreSendRequestHeaders(object sender, EventArgs e)
		{
            HttpApplication application = (HttpApplication)sender;
            IFormsAuthenticationService authenticationService = IocContainer.Instance.Resolve<IFormsAuthenticationService>();
            authenticationService.SlideExpire(application.Context);
		}	
		

		void application_AuthorizeRequest(object sender, EventArgs e)
		{
			HttpApplication application = (HttpApplication)sender;
			HttpRequest request = application.Context.Request;
			
			_logger.DebugFormat(CultureInfo.InvariantCulture, "AuthorizeRequest {0}", request.AppRelativeCurrentExecutionFilePath);

			try
			{
                IFormsAuthenticationService authenticationService = IocContainer.Instance.Resolve<IFormsAuthenticationService>();
			    authenticationService.AuthorizeRequest();
			}
			catch (Exception exception)
			{
				_logger.Error(exception);
			}
		}
		

		void application_AcquireRequestState(object sender, EventArgs e)
		{
			HttpApplication application = (HttpApplication)sender;
			HttpContext context = application.Context;
			HttpRequest request = application.Context.Request;

			_logger.DebugFormat(CultureInfo.InvariantCulture, "AcquireRequestState {0}", request.AppRelativeCurrentExecutionFilePath);

            var localeProvider = IocContainer.Instance.Resolve<ILocaleProvider>();
            var webConfiguration = IocContainer.Instance.Resolve<IWebConfiguration>();
            var trackingService = IocContainer.Instance.Resolve<ITrackingService>();

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
		}
	}
}
