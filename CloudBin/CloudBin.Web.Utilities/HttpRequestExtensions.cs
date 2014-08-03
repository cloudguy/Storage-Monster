using System;
using System.Web;
using System.Text.RegularExpressions;
using CloudBin.Core;
using CloudBin.Core.Utilities;
using System.Linq;
using System.Collections.Generic;

namespace CloudBin.Web.Utilities
{
    public static class HttpRequestExtensions
    {
        private static Lazy<IEnumerable<Func<string, bool>>> LazyComparers = new Lazy<IEnumerable<Func<string, bool>>>(() =>
        {
            const string requestPattern = "{0}{1}{2}/";
            string applicationPath = HttpContext.Current.Request.ApplicationPath;
            string pathDelimeter = (applicationPath != null && applicationPath.EndsWith("/")) ? "" : "/";
            string scriptPattern = string.Format(System.Globalization.CultureInfo.InvariantCulture, requestPattern,
                applicationPath,
                pathDelimeter,
                Constants.ScriptsFolderName);
            string contentPattern = string.Format(System.Globalization.CultureInfo.InvariantCulture, requestPattern,
                applicationPath,
                pathDelimeter,
                Constants.ContentFolderName);
            Regex axdRegex = new Regex(Constants.AxdRequestPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return new Func<string, bool>[]
            {
                input => string.Equals(input, scriptPattern, StringComparison.OrdinalIgnoreCase),
                input => string.Equals(input, contentPattern, StringComparison.OrdinalIgnoreCase),
                input => axdRegex.IsMatch(input)
            };
        }, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        //for mono, if option runAllManagedModulesForAllRequests is not available
        public static bool IsScriptOrContentRequest(this HttpRequest request)
        {
            return RequestContext.Current.LookUpValue("is_script_or_content_request", () =>
            {
                foreach (Func<string, bool> comparer in LazyComparers.Value)
                {
                    if (comparer(HttpContext.Current.Request.Path))
                    {
                        return true;
                    }
                }
                return false;
            });
        }

        public static bool IsAjaxRequest(this HttpRequest httpRequest)
        {
            Verify.NotNull(() => httpRequest);
// ReSharper disable AssignNullToNotNullAttribute
            return string.Equals(httpRequest.Headers.GetValues(Constants.AjaxHeaderName).FirstOrDefault(), Constants.AjaxHeaderValue, StringComparison.OrdinalIgnoreCase);
// ReSharper restore AssignNullToNotNullAttribute
        }
    }
}
