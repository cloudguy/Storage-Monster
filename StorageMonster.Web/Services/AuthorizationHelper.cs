using StorageMonster.Web.Models;
using StorageMonster.Web.Models.Account;
using StorageMonster.Web.Properties.ViewsResources;
using StorageMonster.Web.Services.Extensions;
using System;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Services
{
    public static class AuthorizationHelper
    {
        public static void RedirectToLogon(HttpRequest request, HttpResponse response)
        {
            if (!request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                response.Redirect("~/account/logon");
            }
            else
            {
                string returnUrl = Uri.EscapeUriString(request.Url.PathAndQuery);
                if (returnUrl.Equals("/", StringComparison.OrdinalIgnoreCase))
                    response.Redirect("~/account/logon");
                else
                    response.Redirect("~/account/logon?returnUrl=" + returnUrl);
            }
        }

        public static JsonResult GetAuthAjaxResult(ControllerContext actionContext, LogOnModel model, bool authorized)
        {
            UrlHelper u = new UrlHelper(actionContext.RequestContext);
            string logOnPage = null;
            if (!authorized)
                logOnPage = actionContext.Controller.RenderViewToString("~/Views/Account/Controls/AjaxLogonFormControl.cshtml", model);
            return new JsonResult
            {
                Data = new AjaxAuthModel
                {
                    LogonUrl = u.Action("LogOn", "Account"),
                    Authorized = authorized,
                    LogOnPage = logOnPage,
                    LogOnTitle = AccountResources.LogOnTitle
                },
                ContentEncoding = System.Text.Encoding.UTF8,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public static JsonResult GetAuthAjaxResult(ControllerContext actionContext, bool authorized)
        {
            return GetAuthAjaxResult(actionContext, new LogOnModel(), authorized);
        }
    }
}