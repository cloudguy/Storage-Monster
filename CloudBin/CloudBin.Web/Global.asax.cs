using CloudBin.Core;
using CloudBin.Core.Configuration;
using CloudBin.Core.Utilities;
using CloudBin.IoC.Castle.Windsor;
using CloudBin.Web.Services.Configuration;
using CloudBin.Web.Utilities;
using Common.Logging;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DependencyResolver = System.Web.Mvc.DependencyResolver;
using RequestContext = CloudBin.Core.RequestContext;
using System;
using System.Web.Optimization;
using CloudBin.Data;
using System.Globalization;

namespace CloudBin.Web
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
            Type type = Type.GetType(dependencyContainerConfiguration.DependencyContainerType);
            Verify.NotNull(() => type, string.Format(CultureInfo.InvariantCulture, "Dependency container type not found"));
// ReSharper disable AssignNullToNotNullAttribute
            IDependencyContainer container = (IDependencyContainer)Activator.CreateInstance(type);
// ReSharper restore AssignNullToNotNullAttribute
            DependencyContainer.Initialize(container.RegisterFromAppConfig().RegisterTypesInDirectory(typeof(IController), Assembly.GetExecutingAssembly().Directory()));
            DependencyResolver.SetResolver(new Utilities.DependencyResolver(DependencyContainer.Current));
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
            BundlesConfiguration.RegisterBundles(BundleTable.Bundles);

            Logger.Debug("Registering case insensitive view engine");
            CaseInsensitiveViewEngine.Register(ViewEngines.Engines);

            Logger.Debug("Initializing database session manager");
            IDatabaseSessionManager databaseSessionManager = DependencyResolver.Current.GetService<IDatabaseSessionManager>();
            IDatabaseConfiguration databaseConfiguration = DependencyResolver.Current.GetService<IDatabaseConfiguration>();
            databaseSessionManager.Initialize(databaseConfiguration);
            DatabaseSessionManager.SetDatabaseSessionManager(databaseSessionManager);
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
