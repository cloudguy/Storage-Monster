using BundleTransformer.Core;
using BundleTransformer.Core.Transformers;
using CloudBin.Core;
using CloudBin.Core.Utilities;
using CloudBin.Web.Core.Bundling;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Optimization;

namespace CloudBin.Web.BundleTransformer
{
    public sealed class BundleProvider : IBundleProvider
    {
        private readonly Lazy<Type> _bundleHandlerType = new Lazy<Type>(() =>
        {
            AssemblyName optimizationAssemblyName = Assembly.GetExecutingAssembly().GetReferencedAssemblies().First(a => a.Name.Equals("System.Web.Optimization", StringComparison.Ordinal));
            Assembly optimizationAssembly = Assembly.Load(optimizationAssemblyName);
            Type bundleHandlerType = optimizationAssembly.GetType("System.Web.Optimization.BundleHandler");
            Verify.NotNull(() => bundleHandlerType, () => new InvalidOperationException("System.Web.Optimization.BundleHandler type not found"));
            return bundleHandlerType;
        });

        void IBundleProvider.RegisterScriptBundle(string bundleName, params string[] scriptVirtualPaths)
        {
            var scriptTransformer = new ScriptTransformer();
            var scriptBundle = new Bundle(BundleHelper.GetBundlePath(bundleName), scriptTransformer);
            foreach (string scriptVirtualPath in scriptVirtualPaths)
            {
                scriptBundle.Include(scriptVirtualPath);
            }
            BundleTable.Bundles.Add(scriptBundle);
        }

        void IBundleProvider.RegisterStyleBundle(string bundleName, params string[] styleVirtualPaths)
        {
            StyleTransformer styleTransformer = new StyleTransformer();
            var stylesBundle = new Bundle(BundleHelper.GetBundlePath(bundleName), styleTransformer);
            foreach (string styleVirtualPath in styleVirtualPaths)
            {
                stylesBundle.Include(styleVirtualPath);
            }
            BundleTable.Bundles.Add(stylesBundle);
        }

        IHtmlString IBundleProvider.GetBundleUrl(string bundleName)
        {
            return Scripts.Url(BundleHelper.GetBundlePath(bundleName));
        }

        bool IBundleProvider.IsBundleRequest()
        {
            return RequestContext.Current.LookUpValue("is_bundle_request", () =>
            {
                if (HttpContext.Current.Handler != null && HttpContext.Current.Handler.GetType() == _bundleHandlerType.Value)
                {
                    return true;
                }

                const string requestPattern = "{0}{1}{2}/";
                string applicationPath = HttpContext.Current.Request.ApplicationPath;
                string pathDelimeter = (applicationPath != null && applicationPath.EndsWith("/")) ? string.Empty : "/";
                string bundlePattern = string.Format(CultureInfo.InvariantCulture, requestPattern,
                    applicationPath,
                    pathDelimeter,
                    Constants.BundlesRootPath);
                return HttpContext.Current.Request.Path.StartsWith(bundlePattern, StringComparison.OrdinalIgnoreCase);
            });
        }

        bool IBundleProvider.EnableOptimizations
        {
            get { return BundleTable.EnableOptimizations; }
            set { BundleTable.EnableOptimizations = value; }
        }

        void IBundleProvider.Initialize()
        {
            IBundleTransformerContext currentContext = global::BundleTransformer.Core.BundleTransformerContext.Current;
            global::BundleTransformer.Core.BundleTransformerContext.Current = new CloudBin.Web.BundleTransformer.BundleTransformerContext(currentContext);
        }
    }
}
