using System.Globalization;
using CloudBin.Core.Utilities;

namespace CloudBin.Web.Core
{
    public static class BundleHelper
    {
        public static string GetBundlePath(string bundleName)
        {
            Verify.NotNullOrWhiteSpace(() => bundleName);
            return string.Format(CultureInfo.InvariantCulture, "~/{0}/{1}", Constants.BundlesRootPath, bundleName);
        }
    }
}
