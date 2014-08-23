using CloudBin.Web.Core.Routing;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudBin.Web.UI.Services.Configuration
{
    internal static class RouteConfiguration
    {
        internal static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(
                "SignIn", // Route name
                "SignIn", // URL with parameters
                new { controller = "Account", action = "SignIn" });

            routes.MapRouteLowercase(
                "SignOut", // Route name
                "SignOut", // URL with parameters
                new { controller = "Account", action = "SignOut" });

            routes.MapRouteLowercase(
                "Register", // Route name
                "Register", // URL with parameters
                new { controller = "Account", action = "Register" });

            routes.MapRouteLowercase(
                "Default", // Route name
                "{controller}/{action}", // URL with parameters
                new {controller = "Main", action = "Index"});
        }
    }
}
