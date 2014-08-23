using CloudBin.Web.Core.Bundling;
using CloudBin.Web.Core.Theming;
using System.Globalization;
using DependencyResolver = System.Web.Mvc.DependencyResolver;

namespace CloudBin.Web.UI.Services.Configuration
{
    internal static class BundlesConfiguration
    {
        internal static void RegisterBundles()
        {
            IBundleProvider bundleProvider = (IBundleProvider)DependencyResolver.Current.GetService(typeof(IBundleProvider));
            IThemeProvider themeProvider = (IThemeProvider)DependencyResolver.Current.GetService(typeof(IThemeProvider));
            bundleProvider.EnableOptimizations = true;
            bundleProvider.Initialize();
            bundleProvider.RegisterScriptBundle("jquerycommon.js", new[]
            {
                "~/Scripts/vendor/jquery-{version}.min.js",
                "~/Scripts/vendor/jquery.validate.min.js",
                "~/Scripts/vendor/jquery.validate.unobtrusive.min.js",
                "~/Scripts/vendor/bootstrap.min.js"
            });

            themeProvider.Initialize(Resources.ThemeResources.ResourceManager);
            foreach (Theme theme in themeProvider.SupportedThemes)
            {
                RegisterThemeBundle(theme, bundleProvider);
            }
        }

        private static void RegisterThemeBundle(Theme theme, IBundleProvider bundleProvider)
        {
            const string bootstrapCss = "~/Content/css/bootstrap.css";
            const string kendoBootstrapCss = "~/Content/css/kendo.common-bootstrap.min.css";
            const string bootstrapThemePattern = "~/Content/themes/{0}/bootstrap-theme.css";
            const string kendoThemePattern = "~/Content/themes/{0}/kendo.{0}.min.css";

            bundleProvider.RegisterStyleBundle(theme.BundleName, new[]
            {
                bootstrapCss,
                kendoBootstrapCss,
                string.Format(CultureInfo.InvariantCulture, bootstrapThemePattern, theme.Name),
                string.Format(CultureInfo.InvariantCulture, kendoThemePattern, theme.Name)
            });
        }
    }
}
