

using System;
using System.Globalization;
using System.Text;
using System.Web;
using Common.Logging;

namespace StorageMonster.Web.Services.Security
{
    public static class ForbiddenRequestsLogger
    {
        private static readonly ILog ForbiddenLogger = LogManager.GetLogger("ForbiddenRequests");

        private const string UnknownFieldStab = "--unknown--";

        private static string BuildRequestInfo(string url,
            string rawUrl,
            string requestType,
            string userHostAddress,
            string userHostName,
            string urlReferer,
            string userAgent,
            Principal user)
        {
            StringBuilder builder = new StringBuilder("----FORBIDDEN REQUEST----");
            builder.AppendLine("Request information: ");
            builder.Append("Url:");
            builder.Append(url);
            builder.AppendLine();

            builder.Append("Raw url: ");
            builder.Append(rawUrl);
            builder.AppendLine();

            builder.Append("Request type: ");
            builder.Append(requestType);
            builder.AppendLine();

            builder.Append("User host address: ");
            builder.Append(userHostAddress);
            builder.AppendLine();

            builder.Append("User host name: ");
            builder.Append(userHostName);
            builder.AppendLine();

            builder.Append("Url referer: ");
            builder.Append(urlReferer);
            builder.AppendLine();

            builder.Append("User agent: ");
            builder.Append(userAgent);
            builder.AppendLine();

            if (user != null && user.Identity != null)
            {
                builder.Append("User identity: ");
                builder.AppendFormat(CultureInfo.InvariantCulture, "ID [{0}] Email [{1}]", user.Identity.UserId, user.Identity.Email);
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public static void LogRequest(HttpRequestBase request, Exception exception)
        {
            string requestInfo = string.Empty;

            if (request != null)
            {
                string url = request.Url != null ? request.Url.ToString() : UnknownFieldStab;
                string rawUrl = request.RawUrl ?? UnknownFieldStab;
                string requestType = request.RequestType ?? UnknownFieldStab;
                string userHostAddress = request.UserHostAddress ?? UnknownFieldStab;
                string userHostName = request.UserHostName ?? UnknownFieldStab;
                string urlReferer = request.UrlReferrer != null ? request.UrlReferrer.ToString() : UnknownFieldStab;
                string userAgent = request.UserAgent ?? UnknownFieldStab;

                requestInfo = BuildRequestInfo(url, rawUrl, requestType, userHostAddress, userHostName, urlReferer, userAgent, request.RequestContext.HttpContext.User as Principal);
            }

            ForbiddenLogger.Warn(requestInfo, exception);
        }

        public static void LogRequest(HttpRequest request, Exception exception)
        {
            string requestInfo = string.Empty;

            if (request != null)
            {
                string url = request.Url.ToString();
                string rawUrl = request.RawUrl;
                string requestType = request.RequestType;
                string userHostAddress = request.UserHostAddress ?? UnknownFieldStab;
                string userHostName = request.UserHostName ?? UnknownFieldStab;
                string urlReferer = request.UrlReferrer != null ? request.UrlReferrer.ToString() : UnknownFieldStab;
                string userAgent = request.UserAgent ?? UnknownFieldStab;

                requestInfo = BuildRequestInfo(url, rawUrl, requestType, userHostAddress, userHostName, urlReferer, userAgent, request.RequestContext.HttpContext.User as Principal);
            }

            ForbiddenLogger.Warn(requestInfo, exception);
        }
    }
}