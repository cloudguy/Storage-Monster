using System;
using System.Net;
using System.Web;
using StorageMonster.Web.Services.ActionAnnotations;
using System.Web.Mvc;
using StorageMonster.Web.Services.Security;

namespace StorageMonster.Web.Controllers
{
    [TempDataTransfer]
    public abstract class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!(filterContext.HttpContext.User.Identity is Identity))
                throw new InvalidOperationException("Storage monster custom identity is supported only.");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            //HttpException httpEx = filterContext.Exception as HttpException;
            //if (httpEx != null && httpEx.GetHttpCode() == (int) HttpStatusCode.Forbidden)
            //{
            //    //if (code == HttpStatusCode.Unauthorized)
            //    //{
            //    //    if (!application.Context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            //    //    {
            //    //        application.Response.Redirect("~/account/logon");
            //    //        return;
            //    //    }
            //    //    string returnUrl = Uri.EscapeUriString(application.Context.Request.Url.PathAndQuery);
            //    //    if (returnUrl.Equals("/", StringComparison.OrdinalIgnoreCase))
            //    //        application.Response.Redirect("~/account/logon");
            //    //    else
            //    //        application.Response.Redirect("~/account/logon?returnUrl=" + returnUrl);
            //    //}
            //}
            ////HttpException((int)HttpStatusCode.Forbidden, error);
            base.OnException(filterContext);
        }
    }
}
