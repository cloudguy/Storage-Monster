using CloudBin.Web.Utilities;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudBin.Web.Services.Configuration
{
    internal static class RouteConfiguration
    {
        internal static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(
                "Default", // Route name
                "{controller}/{action}", // URL with parameters
                new {controller = "Main", action = "Index"} // Parameter defaults
                );
        }
    }
}
