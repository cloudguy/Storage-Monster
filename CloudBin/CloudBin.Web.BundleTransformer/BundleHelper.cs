using CloudBin.Core.Utilities;
using System.Globalization;

namespace CloudBin.Web.BundleTransformer
{
    internal static class BundleHelper
    {
        internal static string GetBundlePath(string bundleName)
        {
            Verify.NotNullOrWhiteSpace(() => bundleName);
            return string.Format(CultureInfo.InvariantCulture, "~/{0}/{1}", Constants.BundlesRootPath, bundleName);
        }
    }
}
