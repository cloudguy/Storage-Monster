using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using Common.Logging;
using StorageMonster.Common;
using StorageMonster.DB;
using StorageMonster.DB.Repositories;
using StorageMonster.Services;
using StorageMonster.Services.Security;
using StorageMonster.Web.Models;
using StorageMonster.Web.Services;
using StorageMonster.Web.Services.Validation;
using RequestContext = StorageMonster.Util.RequestContext;
using User = StorageMonster.DB.Domain.User;


namespace StorageMonster.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {		
        protected static readonly ILog Logger = LogManager.GetLogger(typeof(MvcApplication));
		protected static readonly ILog ForbiddenLogger = LogManager.GetLogger("ForbiddenRequests");

        

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "NotFound", // Route name
                "NotFound", // URL with parameters
                new { controller = "Home", action = "NotFound" } // Parameter defaults
            );

            routes.MapRoute(
                "Forbidden", // Route name
                "Forbidden", // URL with parameters
                new { controller = "Home", action = "Forbidden" } // Parameter defaults
            );


            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        public override string GetVaryByCustomString(HttpContext context, string value)
        {
            if (value.Equals(Constants.LocaleCacheDisableKey))
            {
                LocaleData localeData = RequestContext.GetValue<LocaleData>(RequestContext.LocaleKey);
                return localeData == null ? Thread.CurrentThread.CurrentUICulture.Name : localeData.ShortName;
            }

            return base.GetVaryByCustomString(context, value);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            IoCcontainer.ConfigureStructureMap();
            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(IoCcontainer.Instance));
            ILocaleProvider localeProvider = IoCcontainer.Instance.Resolve<ILocaleProvider>();
            LocaleData defaultLocale = new LocaleData
                {
                    Culture = CultureInfo.GetCultureInfo("en"),
                    FullName = "English",
                    ShortName = "en"
                };

            LocaleData russianLocale = new LocaleData
                {
                    Culture = CultureInfo.GetCultureInfo("ru"),
                    FullName = "Русский",
                    ShortName = "ru"
                };

            localeProvider.Init(new[] { defaultLocale, russianLocale }, defaultLocale);

            RegisterRoutes(RouteTable.Routes);

            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MinStringLengthAttribute), typeof(MinStringLengthValidator));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(PropertiesMustMatchAttribute), typeof(PropertiesMustMatchValidator));
        }

        protected void Application_AuthorizeRequest(object sender, EventArgs e)
        {
#warning filter content))
            try
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null)
                {
                    SetUser(Context, null, null, false);
                    return;
                }
                
                //Extract the forms authentication cookie
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket == null)
                {
                    SetUser(Context, null, null, false);
                    return;
                }

                var sessionRepo = IoCcontainer.Instance.Resolve<ISessionRepository>();
                var session = sessionRepo.GetSessionByToken(authTicket.UserData);
                if (session == null || (session.Expiration != null && session.Expiration <= DateTime.UtcNow))
                {
                    SetUser(Context, null, null, false);
                    return;
                }
                var userRepo = IoCcontainer.Instance.Resolve<IUserRepository>();
                User user = userRepo.GetUserBySessionToken(session);

                var userRolesRepo = IoCcontainer.Instance.Resolve<IUserRoleRepository>();
                string[] roles = userRolesRepo.GetRolesForUser(user).Select(r => r.Role).ToArray();
                SetUser(Context, user, roles, true);
            }
            catch (Exception exception)
            {
                SetUser(Context, null, null, false);
                Logger.Error(exception);
            }
        }
       

        /*protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }*/

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var connectionProvider = IoCcontainer.Instance.Resolve<IConnectionProvider>();
            connectionProvider.CloseCurentConnection();
            IoCcontainer.Instance.CleanUpRequestResources();
        }

        public static bool IsAjaxRequest(HttpRequest httpRequest)
        {
            var headers = httpRequest.Headers;
            if (!headers.AllKeys.Contains("X-Requested-With") || headers.GetValues("X-Requested-With").FirstOrDefault() != "XMLHttpRequest")
                return false;

            return true;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
#warning check nulls
            Exception ex = Server.GetLastError();
            if (ex is HttpException)
            {
                if (((HttpException)ex).GetHttpCode() == 404)
                {
                    if (IsAjaxRequest(HttpContext.Current.Request))
                    {
                        var jsserializer = new JavaScriptSerializer();
                        string response = jsserializer.Serialize(new AjaxErrorModel
                            {
#warning localization
                                Error = "Not Found"
                            });
                        HttpContext.Current.Response.ClearContent();
                        HttpContext.Current.Response.Write(response);
                        HttpContext.Current.Response.End();
                    }
                    return;
                }
                if (((HttpException)ex).GetHttpCode() == 403)
                {
                    if (IsAjaxRequest(HttpContext.Current.Request))
                    {
                        var jsserializer = new JavaScriptSerializer();
                        string response = jsserializer.Serialize(new AjaxErrorModel
                            {
                                Error = Properties.ValidationResources.AjaxAccessDenied
                            });
                        HttpContext.Current.Response.ClearContent();
                        HttpContext.Current.Response.Write(response);
                        HttpContext.Current.Response.End();
                    }
                    ForbiddenLogger.Warn(ex);
                    return;
                }
            }

            if (IsAjaxRequest(HttpContext.Current.Request))
            {
                var jsserializer = new JavaScriptSerializer();
                string response = jsserializer.Serialize(new AjaxErrorModel
                {
                    Error = Properties.ValidationResources.AjaxServerError
                });
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.Write(response);
                HttpContext.Current.Response.End();
            }
            Logger.Error(ex);
        }


        private static void SetUser(HttpContext context, User user, string[] roles, bool isAutheticated)
        {
            Identity identity = new Identity(user, isAutheticated);
            Principal principal = new Principal(identity, roles);
            context.User = principal;

            
            string langName = string.Empty;
            if (user == null)
            {
                if (HttpContext.Current.Request.UserLanguages != null && HttpContext.Current.Request.UserLanguages.Length > 0 && HttpContext.Current.Request.UserLanguages[0].Length >= 2)
                    langName = HttpContext.Current.Request.UserLanguages[0].Substring(0, 2);
            }
            else
            {
                langName = user.Locale;
            }

            var localeProvider = IoCcontainer.Instance.Resolve<ILocaleProvider>();
            LocaleData locale = localeProvider.GetCultureByName(langName);

            RequestContext.SetValue(RequestContext.LocaleKey, locale);
        }



        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
#warning remove test shit
            var ci = CultureInfo.GetCultureInfo("ru");
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ru");

            var localeProvider = IoCcontainer.Instance.Resolve<ILocaleProvider>();
            LocaleData locale = localeProvider.GetCultureByName("ru");
            RequestContext.SetValue(RequestContext.LocaleKey, locale);

            /*LocaleData locale = RequestContext.GetValue<LocaleData>(RequestContext.LocaleKey);
            Thread.CurrentThread.CurrentUICulture = locale.Culture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(locale.Culture.Name);*/
        }

    }
}