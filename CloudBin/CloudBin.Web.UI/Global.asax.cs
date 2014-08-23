using CloudBin.Core;
using CloudBin.Core.Configuration;
using CloudBin.Core.Utilities;
using CloudBin.Data;
using CloudBin.Web.Core;
using CloudBin.Web.Core.Configuration;
using CloudBin.Web.Core.ViewEngines;
using CloudBin.Web.UI.Services.Configuration;
using Common.Logging;
using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DependencyResolver = System.Web.Mvc.DependencyResolver;
using RequestContext = CloudBin.Core.RequestContext;

namespace CloudBin.Web.UI
{
    public class MvcApplication : HttpApplication
    {
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        protected void Application_Start()
        {
            Logger.Debug("Initializing IoC and controller factory");
            IDependencyContainerConfiguration dependencyContainerConfiguration = new DependencyContainerXmlConfiguration();
            IDependencyContainer container = DependencyContainerFactory.CreateContainer(dependencyContainerConfiguration);
            DependencyContainer.Initialize(container.RegisterFromAppConfig().RegisterTypesInDirectory(typeof(IController), Assembly.GetExecutingAssembly().Directory()));
            DependencyResolver.SetResolver(new Core.DependencyResolver(DependencyContainer.Current));
            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(DependencyContainer.Current));

            if (DependencyResolver.Current.GetService<IWebConfiguration>().RemoveVersionHeaders)
            {
                MvcHandler.DisableMvcResponseHeader = true;
            }

            Logger.Debug("Settings request context provider");
            IRequestContextProvider requestContextProvider = DependencyResolver.Current.GetService<IRequestContextProvider>();
            RequestContext.SetProvider(requestContextProvider);

            Logger.Debug("Initializing locales");
            IGlobalizationConfiguration globalizationConfiguration = DependencyResolver.Current.GetService<IGlobalizationConfiguration>();
            ILocaleProvider localeProvider = DependencyResolver.Current.GetService<ILocaleProvider>();
            localeProvider.Initialize(globalizationConfiguration);

            Logger.Debug("Registering routes");
            RouteConfiguration.RegisterRoutes(RouteTable.Routes);

            Logger.Debug("Registering bundles");
            BundlesConfiguration.RegisterBundles();

            Logger.Debug("Registering case insensitive view engine");
            CaseInsensitiveViewEngine.Register(ViewEngines.Engines);

            Logger.Debug("Initializing database session manager");
            IDatabaseSessionManager databaseSessionManager = DependencyResolver.Current.GetService<IDatabaseSessionManager>();
            IDatabaseConfiguration databaseConfiguration = DependencyResolver.Current.GetService<IDatabaseConfiguration>();
            databaseSessionManager.Initialize(databaseConfiguration);
            DatabaseSessionManager.SetDatabaseSessionManager(databaseSessionManager);

            Logger.Debug("Initializing validation");
            FluentValidation.Mvc.FluentValidationModelValidatorProvider.Configure();
        }

        protected void Application_End()
        {
            DependencyContainer.ShutDown();
        }

        protected void Application_Error()
        {
            Exception error = Server.GetLastError();
            if (error != null)
            {
                Logger.Error("Unhandled exception", error);
            }
        }
    }
}
