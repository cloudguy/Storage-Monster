using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StorageMonster.Services.Security;
using StorageMonster.Web.Models;
using StorageMonster.Web.Models.Accounts;

namespace StorageMonster.Web.Services
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class MonsterAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public MonsterAuthorizeAttribute(string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (actionContext.HttpContext.Request.IsAjaxRequest())
                {
#warning rewrite
                    UrlHelper u = new UrlHelper(actionContext.RequestContext);
                    actionContext.Result = new JsonResult
                    {
                        Data = new AjaxUnauthorizedModel
                            {
                                Redirect =  u.Content(FormsAuthentication.LoginUrl),
                                LogOnPage = actionContext.Controller.RenderViewToString<object>("~/Views/Account/LogOnFormControl.ascx", new LogOnModel())
                            },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    return;
                }

                actionContext.Result = new HttpUnauthorizedResult();
                return;
            }

            if (_roles == null)
                return;

            string role = _roles.Where(r => actionContext.HttpContext.User.IsInRole(r)).FirstOrDefault();
            
            if (role == null)
            {
                String error = String.Format(CultureInfo.InvariantCulture, "User {0} requested page {1}", ((Identity)actionContext.HttpContext.User.Identity).Email, actionContext.HttpContext.Request.Path);
                throw new HttpException((int) HttpStatusCode.Unauthorized, error);
            }

        }
    }
}

