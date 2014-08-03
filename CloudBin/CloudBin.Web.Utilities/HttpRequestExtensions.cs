using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using CloudBin.Core;
using CloudBin.Core.Utilities;
using System.Linq;
using System.Collections.Generic;

namespace CloudBin.Web.Utilities
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
