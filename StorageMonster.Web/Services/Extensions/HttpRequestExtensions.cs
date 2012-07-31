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
// ReSharper disable AssignNullToNotNullAttribute
			if (!headers.AllKeys.Contains("X-Requested-With") || headers.GetValues("X-Requested-With").FirstOrDefault() != "XMLHttpRequest")
// ReSharper restore AssignNullToNotNullAttribute
				return false;

			return true;
		}
	}
}
