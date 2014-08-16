using System.Linq;
using System.Web.Optimization;
using BundleTransformer.Core.PostProcessors;
using BundleTransformer.Core.Transformers;
using CloudBin.Web.Core;
using CloudBin.Web.Core.Bundling;
using Microsoft.Ajax.Utilities;
using DependencyResolver = System.Web.Mvc.DependencyResolver;

namespace CloudBin.Web.UI.Services.Configuration
{
    internal static class BundlesConfiguration
    {
        internal static void RegisterBundles(BundleCollection bundles)
        {
            IBundleProvider provider = (IBundleProvider) DependencyResolver.Current.GetService(typeof (IBundleProvider));
            provider.EnableOptimizations = true;
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

        private static Bundle RemoveJsMinifier(this Bundle bundle)
        {
            IBundleTransform jsMinifier = bundle.Transforms.FirstOrDefault(t => t is JsMinify);
            if (jsMinifier != null)
            {
                bundle.Transforms.Remove(jsMinifier);
            }
            return bundle;
        }

        private static Bundle RemoveCssMinifier(this Bundle bundle)
        {
            IBundleTransform cssMinifier = bundle.Transforms.FirstOrDefault(t => t is CssMinify);
            if (cssMinifier != null)
            {
                bundle.Transforms.Remove(cssMinifier);
            }
            return bundle;
        }
    }
}
