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
                "~/Scripts/jquery-{version}.min.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js").RemoveJsMinifier());

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
    }
}
