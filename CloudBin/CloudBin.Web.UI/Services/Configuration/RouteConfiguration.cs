using System.Web.Mvc;
using System.Web.Routing;
using CloudBin.Web.Core;

namespace CloudBin.Web.UI.Services.Configuration
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
