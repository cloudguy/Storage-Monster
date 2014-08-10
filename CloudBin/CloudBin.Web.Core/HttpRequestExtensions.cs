using CloudBin.Core.Utilities;
using System;
using System.Linq;
using System.Web;

namespace CloudBin.Web.Core
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest httpRequest)
        {
            Verify.NotNull(() => httpRequest);
// ReSharper disable AssignNullToNotNullAttribute
            return string.Equals(httpRequest.Headers.GetValues(Constants.AjaxHeaderName).FirstOrDefault(), Constants.AjaxHeaderValue, StringComparison.OrdinalIgnoreCase);
// ReSharper restore AssignNullToNotNullAttribute
        }
    }
}
