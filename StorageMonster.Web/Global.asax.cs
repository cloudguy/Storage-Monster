using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Common.Logging;
using StorageMonster.Common.DataAnnotations;
using StorageMonster.Plugin;
using StorageMonster.Services;
using StorageMonster.Web.Models;
using StorageMonster.Web.Properties;
using StorageMonster.Web.Services;
using StorageMonster.Web.Services.Configuration;
using StorageMonster.Web.Services.Extensions;
using StorageMonster.Web.Services.Routing;
using StorageMonster.Web.Services.Security;
using StorageMonster.Web.Services.Validation;

namespace StorageMonster.Web
{
    public class MonsterApplication : HttpApplication
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MonsterApplication));

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content/*");

            routes.MapRouteLowercase(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        private static void RegisterLocales(ILocaleProvider localeProvider)
        {
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
        }

        protected static void InitializePlugins()
        {
            IStoragePluginsService storageSerive = DependencyResolver.Current.GetService<IStoragePluginsService>();
            storageSerive.ResetStorages();
            IEnumerable<IStoragePlugin> storagePlugins = DependencyResolver.Current.GetServices<IStoragePlugin>();
            if (storagePlugins.FirstOrDefault() == null)
            {
                Logger.Warn("No storage plugins found");
                return;
            }
            storageSerive.InitStorges(storagePlugins);
        }

        protected static void InitializeSweeper()
        {
#warning test this
            WebConfigurationSection configuration = (WebConfigurationSection)ConfigurationManager.GetSection(WebConfigurationSection.SectionLocation);

            if (!configuration.Sweeper.Enabled)
                return;

            ILog sweeperLogger = LogManager.GetLogger(typeof(ISweeper));

            ISweeper sweeper = DependencyResolver.Current.GetService<ISweeper>();
            TimeSpan timeout = TimeSpan.FromMinutes(configuration.Sweeper.SweeperTimeout);
            Thread sweeperThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(timeout);
                    try
                    {
                        sweeper.CleanUp(true);
                    }
                    catch (Exception ex)
                    {
                        sweeperLogger.Error(ex);
                    }
                }
            });
            sweeperThread.IsBackground = true;
            sweeperThread.Start();
        }

        protected void Application_Start()
        {
            Logger.Info("Storage Monster is starting...");
            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                Logger.Error("Initialization failed", ex);
                throw;
            }
            Logger.Info("Storage Monster started");
        }

        private void Initialize()
        {
            Logger.Trace("Registering areas");
            AreaRegistration.RegisterAllAreas();

            Logger.Trace("Registering routes");
            RegisterRoutes(RouteTable.Routes);

            Logger.Trace("Registering case insensitive view engine");
            CaseInsensitiveViewEngine.Register(ViewEngines.Engines);

            Logger.Trace("Initializing IoC");
            StructureMapIoC.CreateContainer();
            DependencyResolver.SetResolver(new MonsterDependencyResolver(IocContainer.Instance));

            Logger.Trace("Initializing locales");
            RegisterLocales(DependencyResolver.Current.GetService<ILocaleProvider>());

            Logger.Trace("Initializing time zones provider");
            DependencyResolver.Current.GetService<ITimeZonesProvider>().Init();

            Logger.Trace("Initializing icon provider");
            DependencyResolver.Current.GetService<IIconProvider>().Init();

            Logger.Trace("Initializing validators");
            var oldValidatorProvider = ModelValidatorProviders.Providers.Single(p => p is DataAnnotationsModelValidatorProvider);
            ModelValidatorProviders.Providers.Remove(oldValidatorProvider);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            ModelValidatorProviders.Providers.Add(new DataAnnotationsModelValidatorProvider());
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
        }


        private void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex == null)
                return;

            HttpException httpException = ex as HttpException;
            string reponseString = null;
            string contentType = null;
            bool loggingRequired = true;
            if (httpException != null)
            {
                if (httpException.GetHttpCode() == (int)HttpStatusCode.NotFound)
                {
                    if (HttpContext.Current.Request.IsAjaxRequest())
                    {
                        var jsserializer = new JavaScriptSerializer();
                        reponseString = jsserializer.Serialize(new AjaxErrorModel
                        {
                            Error = ValidationResources.AjaxNotFound
                        });
                        contentType = "application/json";
                    }
                    loggingRequired = false;
                }
                if (httpException.GetHttpCode() == (int)HttpStatusCode.Forbidden)
                {
                    if (HttpContext.Current.Request.IsAjaxRequest())
                    {
                        var jsserializer = new JavaScriptSerializer();
                        reponseString = jsserializer.Serialize(new AjaxErrorModel
                        {
                            Error = ValidationResources.AjaxAccessDenied
                        });
                        contentType = "application/json";
                    }
                    ForbiddenRequestsLogger.LogRequest(Request, ex);
                    loggingRequired = false;
                }
            }

            if (loggingRequired)
                Logger.Error(ex);


            if (reponseString != null)
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.Write(reponseString);
                    if (contentType != null)
                        HttpContext.Current.Response.ContentType = contentType;
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    HttpContext.Current.Response.End();
                }
            }
        }

        public static void RedirectToLogon(HttpRequest request, HttpResponse response)
        {
            if (!request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                response.Redirect("~/account/logon");
            }
            else
            {
                string returnUrl = Uri.EscapeUriString(request.Url.PathAndQuery);
                if (returnUrl.Equals("/", StringComparison.OrdinalIgnoreCase))
                    response.Redirect("~/account/logon");
                else
                    response.Redirect("~/account/logon?returnUrl=" + returnUrl);
            }
        }
    }
}