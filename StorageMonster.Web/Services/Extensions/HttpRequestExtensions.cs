using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageMonster.Web.Services.Extensions
{
	public static class HttpRequestExtensions
	{
		public static bool IsAjaxRequest(this HttpRequest httpRequest)
		{
			if (httpRequest == null)
				throw new ArgumentNullException("httpRequest");

			var headers = httpRequest.Headers;
			if (!headers.AllKeys.Contains("X-Requested-With") || headers.GetValues("X-Requested-With").FirstOrDefault() != "XMLHttpRequest")
				return false;

			return true;
		}
	}
}
