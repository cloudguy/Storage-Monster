using System.Linq;
using System.Web.Optimization;
using CloudBin.Web.Core;

namespace CloudBin.Web.UI.Services.Configuration
{
    internal static class BundlesConfiguration
    {
        internal static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle(BundleHelper.GetBundlePath("jquerycommon.js")).Include(
                "~/Scripts/vendor/jquery-{version}.min.js",
                "~/Scripts/vendor/jquery.validate.min.js",
                "~/Scripts/vendor/jquery.validate.unobtrusive.min.js").RemoveJsMinifier());

            bundles.Add(new StyleBundle(BundleHelper.GetBundlePath("site.css")).Include("~/Content/site.css"));
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
