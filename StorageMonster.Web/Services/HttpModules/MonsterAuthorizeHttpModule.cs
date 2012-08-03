using System;
using System.Net;
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
		protected static readonly ILog Logger = LogManager.GetLogger(typeof(MonsterAuthorizeHttpModule));
		
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
// ReSharper disable MemberCanBeMadeStatic.Local
// ReSharper disable InconsistentNaming
        void application_EndRequest(object sender, EventArgs e)
// ReSharper restore InconsistentNaming
// ReSharper restore MemberCanBeMadeStatic.Local
        {
            HttpApplication application = (HttpApplication)sender;

            //updating locale cookies
            IWebConfiguration webConfiguration = IocContainer.Instance.Resolve<IWebConfiguration>();
            ILocaleProvider localeProvider = IocContainer.Instance.Resolve<ILocaleProvider>();
            var localeData = RequestContext.GetValue<LocaleData>(RequestContext.LocaleKey);
            if (localeData != null)
            {
                HttpCookie localeCookie = new HttpCookie(webConfiguration.LocaleCookieName, RequestContext.GetValue<LocaleData>(RequestContext.LocaleKey).ShortName);
                localeCookie.Expires = DateTime.UtcNow.Add(webConfiguration.LocaleCookieTimeout);
                application.Context.Response.SetCookie(localeCookie);
            }


           
            HttpStatusCode code = (HttpStatusCode)application.Response.StatusCode;
            if (code == HttpStatusCode.Unauthorized)
            {                
                application.Response.Redirect(webConfiguration.LoginUrl);
            }
        }
        

// ReSharper disable MemberCanBeMadeStatic.Local
// ReSharper disable InconsistentNaming
        void application_PreSendRequestHeaders(object sender, EventArgs e)
// ReSharper restore InconsistentNaming
// ReSharper restore MemberCanBeMadeStatic.Local
		{
            HttpApplication application = (HttpApplication)sender;
            IFormsAuthenticationService authenticationService = IocContainer.Instance.Resolve<IFormsAuthenticationService>();
            authenticationService.SlideExpire(application.Context);
		}	
		

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBeMadeStatic.Local
		void application_AuthorizeRequest(object sender, EventArgs e)
// ReSharper restore MemberCanBeMadeStatic.Local
// ReSharper restore InconsistentNaming
		{
			HttpApplication application = (HttpApplication)sender;
			HttpRequest request = application.Context.Request;
			
			Logger.DebugFormat(CultureInfo.InvariantCulture, "AuthorizeRequest {0}", request.AppRelativeCurrentExecutionFilePath);

			try
			{
                IFormsAuthenticationService authenticationService = IocContainer.Instance.Resolve<IFormsAuthenticationService>();
			    authenticationService.AuthorizeRequest();
			}
			catch (Exception exception)
			{
				Logger.Error(exception);
			}
		}
		
	
// ReSharper disable MemberCanBeMadeStatic.Local
// ReSharper disable InconsistentNaming
		void application_AcquireRequestState(object sender, EventArgs e)
// ReSharper restore InconsistentNaming
// ReSharper restore MemberCanBeMadeStatic.Local
		{
			HttpApplication application = (HttpApplication)sender;
			HttpContext context = application.Context;
			HttpRequest request = application.Context.Request;

			Logger.DebugFormat(CultureInfo.InvariantCulture, "AcquireRequestState {0}", request.AppRelativeCurrentExecutionFilePath);

            var localeProvider = IocContainer.Instance.Resolve<ILocaleProvider>();
            var webConfiguration = IocContainer.Instance.Resolve<IWebConfiguration>();

			Identity identity = context.User.Identity as Identity;
			string langName = string.Empty;

            
			if (identity == null || string.IsNullOrEmpty(identity.Locale))
			{
                //user not authenticated, first try is tracking cookie
                //if no cookie, use browser headers
                var cookie = request.Cookies.Get(webConfiguration.LocaleCookieName);
                if (cookie != null)
                    langName = cookie.Value;
                else if (request.UserLanguages != null && request.UserLanguages.Length != 0)				
                    langName = request.UserLanguages[0].Substring(0, 2);				
			}
			else
			{
				langName = identity.Locale;
			}
            
            LocaleData locale = localeProvider.GetCultureByName(langName);
            localeProvider.SetThreadLocale(locale);		
		}
	}
}
