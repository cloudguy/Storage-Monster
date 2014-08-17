using CloudBin.Web.Core.Bundling;
using DependencyResolver = System.Web.Mvc.DependencyResolver;

namespace CloudBin.Web.UI.Services.Configuration
{
    internal static class BundlesConfiguration
    {
        internal static void RegisterBundles()
        {
            IBundleProvider provider = (IBundleProvider) DependencyResolver.Current.GetService(typeof (IBundleProvider));
            provider.EnableOptimizations = true;
			provider.Initialize ();
            provider.RegisterScriptBundle("jquerycommon.js", new[]
            {
                "~/Scripts/vendor/jquery-{version}.js",
                "~/Scripts/vendor/jquery.validate.js",
                "~/Scripts/vendor/jquery.validate.unobtrusive.js"
            });

            provider.RegisterStyleBundle("site.css", new[]
            {
                "~/Content/site.css"
            });

            //BundleTable.EnableOptimizations = true;

            //var scriptTransformer = new JsTransformer();
            //var scriptbundle = new Bundle(BundleHelper.GetBundlePath("jquerycommon.js"), scriptTransformer);
            //scriptbundle.Include(
            //    "~/Scripts/vendor/jquery-{version}.js",
            //    "~/Scripts/vendor/jquery.validate.js",
            //    "~/Scripts/vendor/jquery.validate.unobtrusive.js");
            //bundles.Add(scriptbundle);


            /*  bundles.Add(new ScriptBundle(BundleHelper.GetBundlePath("jquerycommon.js")).Include(
                "~/Scripts/vendor/jquery-{version}.min.js",
                "~/Scripts/vendor/jquery.validate.min.js",
                "~/Scripts/vendor/jquery.validate.unobtrusive.min.js").RemoveJsMinifier());
            */

            /*
            var u = new UrlRewritingCssPostProcessor();
            u.UseInDebugMode = true;
            var styleTransformer = new StyleTransformer(new IPostProcessor[] { u});
            //styleTransformer.
            var st = new StyleBundle(BundleHelper.GetBundlePath("site.css")).Include("~/Content/site.css");
            st.Transforms.Add(styleTransformer);
            bundles.Add(st);
             * */


            //var cssTransformer = new CssTransformer();
            //var commonStylesBundle = new Bundle(BundleHelper.GetBundlePath("site.css"), cssTransformer);
            //commonStylesBundle.Include("~/Content/site.css");
            //bundles.Add(commonStylesBundle);
        }
    }
}
