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
            HttpStatusCode code = (HttpStatusCode)application.Response.StatusCode;
            if (code == HttpStatusCode.Unauthorized)
            {
                IWebConfiguration webConfiguration = IocContainer.Instance.Resolve<IWebConfiguration>();
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

			if (Logger.IsDebugEnabled)
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

			if (Logger.IsDebugEnabled)
				Logger.DebugFormat(CultureInfo.InvariantCulture, "AcquireRequestState {0}", request.AppRelativeCurrentExecutionFilePath);

			Identity identity = context.User.Identity as Identity;
			string langName = string.Empty;
#warning add cookie
			if (identity == null || string.IsNullOrEmpty(identity.Locale))
			{
				if (request.UserLanguages != null && request.UserLanguages.Length != 0)
				{
                    langName = request.UserLanguages[0].Substring(0, 2);
				}
			}
			else
			{
				langName = identity.Locale;
			}

			var localeProvider = IocContainer.Instance.Resolve<ILocaleProvider>();
			LocaleData locale = localeProvider.GetCultureByName(langName);
			RequestContext.SetValue(RequestContext.LocaleKey, locale);			
			Thread.CurrentThread.CurrentUICulture = locale.Culture;
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(locale.Culture.Name);		
		}
	}
}
