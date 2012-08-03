using System;
using System.Collections.Generic;
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
using StorageMonster.Web.Services.Validation;

namespace StorageMonster.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

#warning Validate antiforgery during ajax
    public class MonsterApplication : HttpApplication
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MonsterApplication));
        private static readonly ILog ForbiddenLogger = LogManager.GetLogger("ForbiddenRequests");

        

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content/*");

            routes.MapRouteLowercase(
                "NotFound", // Route name
                "NotFound", // URL with parameters
                new { controller = "Home", action = "NotFound" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "Forbidden", // Route name
                "Forbidden", // URL with parameters
                new { controller = "Home", action = "Forbidden" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "BadRequest", // Route name
                "BadRequest", // URL with parameters
                new { controller = "Home", action = "BadRequest" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

// ReSharper disable InconsistentNaming
        protected void Application_Start()
// ReSharper restore InconsistentNaming
        {
            Logger.Info("Storage Monster starting...");
            try
            {
                Initialize();
                InitializePlugins();
                InitializeSweeper();
            }
            catch(Exception ex)
            {
                Logger.Error("Initialization failed", ex);
                throw;
            }
        }

        // ReSharper disable UseObjectOrCollectionInitializer
        // ReSharper disable FunctionNeverReturns
        protected static void InitializeSweeper()
        {
            IWebConfiguration webConfig = IocContainer.Instance.Resolve<IWebConfiguration>();

            if (!webConfig.RunSweeper)
                return;

            ILog sweeperLogger = LogManager.GetLogger(typeof(ISweeper));

            ISweeper sweeper = IocContainer.Instance.Resolve<ISweeper>();
            TimeSpan timeout = webConfig.SweeperTimeout;
            Thread sweeperThread = new Thread(() =>
                    {
                        while (true)
                        {
                            Thread.Sleep(timeout);
                            try
                            {
                                sweeper.CleanUp();
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
        // ReSharper restore UseObjectOrCollectionInitializer
        // ReSharper restore FunctionNeverReturns

        protected static void Initialize()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            //mono hack
            CaseInsensitiveViewEngine.Register(ViewEngines.Engines);

            StructureMapIoC.CreateContainer();
            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(IocContainer.Instance));
            ILocaleProvider localeProvider = IocContainer.Instance.Resolve<ILocaleProvider>();
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

            var oldValidatorProvider = ModelValidatorProviders.Providers.Single(p => p is DataAnnotationsModelValidatorProvider);
            ModelValidatorProviders.Providers.Remove(oldValidatorProvider);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            ModelValidatorProviders.Providers.Add(new DataAnnotationsModelValidatorProvider());
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MinPasswordLengthAttribute), typeof(MinPasswordLengthValidator));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(PropertiesMustMatchAttribute), typeof(PropertiesMustMatchValidator));
        }

        protected static void InitializePlugins()
        {
            IStoragePluginsService storageSerive = IocContainer.Instance.Resolve<IStoragePluginsService>();
            storageSerive.ResetStorages();
            IEnumerable<IStoragePlugin> storagePlugins = IocContainer.Instance.GetAllInstances<IStoragePlugin>();
            if (storagePlugins.FirstOrDefault() == null)
            {
                Logger.Warn("No storage plugins found");
                return;
            }
            storageSerive.InitStorges(storagePlugins);
        }

// ReSharper disable InconsistentNaming
        protected void Application_Error(object sender, EventArgs e)
// ReSharper restore InconsistentNaming
        {
            Exception ex = Server.GetLastError();
            if (ex == null)
                return;

            HttpException httpException = ex as HttpException;
            HttpAntiForgeryException antiforgeryException = ex as HttpAntiForgeryException;
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
                        contentType = Constants.JsonContentType;
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
                        contentType = Constants.JsonContentType;
                    }
#warning log path and ip
                    ForbiddenLogger.Warn(ex);
                    loggingRequired = false;
                }
            }

            if (antiforgeryException != null)
            {
                if (HttpContext.Current.Request.IsAjaxRequest())
                {
                    var jsserializer = new JavaScriptSerializer();
                    reponseString = jsserializer.Serialize(new AjaxErrorModel
                    {
                        Error = ValidationResources.AjaxAccessDenied
                    });
                    contentType = Constants.JsonContentType;
                }
#warning log path and ip
                ForbiddenLogger.Warn(ex);
                loggingRequired = false;
            }

            if (loggingRequired)
                Logger.Error(ex);

            if (HttpContext.Current.Request.IsAjaxRequest())
            {
                var jsserializer = new JavaScriptSerializer();
                reponseString = jsserializer.Serialize(new AjaxErrorModel
                {
                    Error = ValidationResources.AjaxServerError
                });

            }
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
    }
}