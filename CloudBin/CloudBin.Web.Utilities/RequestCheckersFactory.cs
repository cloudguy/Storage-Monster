using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CloudBin.Web.Utilities
{
    internal static class RequestCheckersFactory
    {
        internal static Func<HttpContext, bool>[] CreateStaticContentCheckers(HttpContext context)
        {
            const string requestPattern = "{0}{1}{2}/";
            string applicationPath = context.Request.ApplicationPath;
            string pathDelimeter = (applicationPath != null && applicationPath.EndsWith("/")) ? "" : "/";
            string scriptPattern = string.Format(CultureInfo.InvariantCulture, requestPattern,
                applicationPath,
                pathDelimeter,
                Constants.ScriptsFolderName);
            string contentPattern = string.Format(CultureInfo.InvariantCulture, requestPattern,
                applicationPath,
                pathDelimeter,
                Constants.ContentFolderName);
            string bundlePattern = string.Format(CultureInfo.InvariantCulture, requestPattern,
                applicationPath,
                pathDelimeter,
                Constants.BundlesRootPath);
            Regex axdRegex = new Regex(Constants.AxdRequestPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return new Func<HttpContext, bool>[]
            {
                ctx => ctx.Request.Path.StartsWith(bundlePattern, StringComparison.OrdinalIgnoreCase),
                ctx => ctx.Request.Path.StartsWith(scriptPattern, StringComparison.OrdinalIgnoreCase),
                ctx => ctx.Request.Path.StartsWith(contentPattern, StringComparison.OrdinalIgnoreCase),
                ctx => axdRegex.IsMatch(ctx.Request.Path)
            };
        }
    }
}
