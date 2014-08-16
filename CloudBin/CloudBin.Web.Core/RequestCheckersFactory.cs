using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using CloudBin.Web.Core.Bundling;

namespace CloudBin.Web.Core
{
    internal static class RequestCheckersFactory
    {
        internal static Func<HttpContext, bool>[] CreateStaticContentCheckers(HttpContext context)
        {
            const string requestPattern = "{0}{1}{2}/";
            string applicationPath = context.Request.ApplicationPath;
            string pathDelimeter = (applicationPath != null && applicationPath.EndsWith("/")) ? string.Empty : "/";
            string scriptPattern = string.Format(CultureInfo.InvariantCulture, requestPattern,
                applicationPath,
                pathDelimeter,
                Constants.ScriptsFolderName);
            string contentPattern = string.Format(CultureInfo.InvariantCulture, requestPattern,
                applicationPath,
                pathDelimeter,
                Constants.ContentFolderName);
            IBundleProvider bundleProvider = (IBundleProvider)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IBundleProvider));
            Regex axdRegex = new Regex(Constants.AxdRequestPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return new Func<HttpContext, bool>[]
            {
                ctx => bundleProvider.IsBundleRequest(),
                ctx => ctx.Request.Path.StartsWith(scriptPattern, StringComparison.OrdinalIgnoreCase),
                ctx => ctx.Request.Path.StartsWith(contentPattern, StringComparison.OrdinalIgnoreCase),
                ctx => axdRegex.IsMatch(ctx.Request.Path)
            };
        }
    }
}
