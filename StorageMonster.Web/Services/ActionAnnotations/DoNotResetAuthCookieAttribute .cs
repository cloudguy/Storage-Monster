using System;
using System.Web.Security;
using System.Web.Mvc;
using StorageMonster.Services;
using StorageMonster.Web.Services.Configuration;

namespace StorageMonster.Web.Services.ActionAnnotations
{
	///<summary>  
	/// Prevent the auth cookie from being reset for this action, allows you to
	/// have requests that do not reset the login timeout.  
	/// </summary>  
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class DoNotResetAuthCookieAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
            IWebConfiguration webConfig = IocContainer.Instance.Resolve<IWebConfiguration>();
			var response = filterContext.HttpContext.Response;
            response.Cookies.Remove(webConfig.AuthenticationCookiename);
		}
	}
}
