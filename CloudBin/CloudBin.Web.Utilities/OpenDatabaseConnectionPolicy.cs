using CloudBin.Core;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CloudBin.Web.Utilities
{
    public class OpenDatabaseConnectionPolicy : IOpenDatabaseConnectionPolicy
    {
        protected readonly IWebConfiguration WebConfiguration;
        protected readonly object Locker = new object();
        protected volatile Func<HttpContext, bool>[] PolicyCheckerInternal;
        protected Func<HttpContext, bool>[] PolicyCheckers
        {
            get
            {
                if (PolicyCheckerInternal == null)
                {
                    lock (Locker)
                    {
                        if (PolicyCheckerInternal == null)
                        {
                            PolicyCheckerInternal = CreatePolicyCheckers(HttpContext.Current);
                        }
                    }
                }
                return PolicyCheckerInternal;
            }
        }

        protected Func<HttpContext, bool>[] CreatePolicyCheckers(HttpContext context)
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

        public OpenDatabaseConnectionPolicy(IWebConfiguration webConfiguration)
        {
            WebConfiguration = webConfiguration;
        }
        bool IOpenDatabaseConnectionPolicy.DatabaseConnectionRequired(HttpContext context)
        {   
            if (!WebConfiguration.DoNotOpenDbSessionForScriptAndContent)
            {
                return true;
            }
            return RequestContext.Current.LookUpValue("db_open_required", () => !PolicyCheckers.Any(checker => checker(context)));
        }
    }
}