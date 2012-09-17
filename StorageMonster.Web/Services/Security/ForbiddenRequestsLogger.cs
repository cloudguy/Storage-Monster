using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Logging;
using System.Text;
using System.Globalization;

namespace StorageMonster.Web.Services.Security
{
    public static class ForbiddenRequestsLogger
    {
        private static readonly ILog ForbiddenLogger = LogManager.GetLogger(Constants.ForbiddenRequestLoggerName);

        private const string unknownFieldStab = "--unknown--";

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
                string url = request.Url != null ? request.Url.ToString() : unknownFieldStab;
                string rawUrl = request.RawUrl ?? unknownFieldStab;
                string requestType = request.RequestType ?? unknownFieldStab;
                string userHostAddress = request.UserHostAddress ?? unknownFieldStab;
                string userHostName = request.UserHostName ?? unknownFieldStab;
                string urlReferer = request.UrlReferrer != null ? request.UrlReferrer.ToString() : unknownFieldStab;
                string userAgent = request.UserAgent ?? unknownFieldStab;

                requestInfo = BuildRequestInfo(url, rawUrl, requestType, userHostAddress, userHostName, urlReferer, userAgent, request.RequestContext.HttpContext.User as Principal);
            }

            ForbiddenLogger.Warn(requestInfo, exception);            
        }

        public static void LogRequest(HttpRequest request, Exception exception)
        {
            string requestInfo = string.Empty;

            if (request != null)
            {                
                string url = request.Url != null ? request.Url.ToString() : unknownFieldStab;
                string rawUrl = request.RawUrl ?? unknownFieldStab;
                string requestType = request.RequestType ?? unknownFieldStab;
                string userHostAddress = request.UserHostAddress ?? unknownFieldStab;
                string userHostName = request.UserHostName ?? unknownFieldStab;
                string urlReferer = request.UrlReferrer != null ? request.UrlReferrer.ToString() : unknownFieldStab;
                string userAgent = request.UserAgent ?? unknownFieldStab;

                requestInfo = BuildRequestInfo(url, rawUrl, requestType, userHostAddress, userHostName, urlReferer, userAgent, request.RequestContext.HttpContext.User as Principal);
            }

            ForbiddenLogger.Warn(requestInfo, exception);    
        }
    }
}