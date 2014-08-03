using System.Linq;
using System.Web.Optimization;

namespace CloudBin.Web
{
    internal static class BundlesConfiguration
    {
        internal static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/jquerycommon.js").Include(
                "~/Scripts/jquery-{version}.min.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js").RemoveJsMinifier());

            bundles.Add(new StyleBundle("~/bundles/site.css").Include("~/Content/site.css"));
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
