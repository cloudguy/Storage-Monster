using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StorageMonster.Services;
using StorageMonster.Web.Models;
using StorageMonster.Web.Models.Accounts;
using StorageMonster.Web.Services.Configuration;
using StorageMonster.Web.Services.Extensions;
using StorageMonster.Web.Services.Security;

namespace StorageMonster.Web.Services.ActionAnnotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class MonsterAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public MonsterAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.HttpContext.User.Identity.IsAuthenticated)
            {
                IWebConfiguration webConfig = IocContainer.Instance.Resolve<IWebConfiguration>();
                if (actionContext.HttpContext.Request.IsAjaxRequest())
                {
                    UrlHelper u = new UrlHelper(actionContext.RequestContext);
                    actionContext.Result = new JsonResult
                    {
                        Data = new AjaxUnauthorizedModel
                            {
                                Redirect = u.Content(webConfig.LoginUrl),
                                LogOnPage = actionContext.Controller.RenderViewToString<object>("~/Views/Account/LogOnFormControl.ascx", new LogOnModel())
                            },
                        ContentEncoding = System.Text.Encoding.UTF8,
                        ContentType = Constants.JsonContentType,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    return;
                }

               // actionContext.Result = new RedirectResult(webConfig.LoginUrl);
                actionContext.Result = new HttpUnauthorizedResult();
                return;
            }

            if (_roles == null)
                return;

            string role = _roles.Where(r => actionContext.HttpContext.User.IsInRole(r)).FirstOrDefault();
            
            if (role == null)
            {
                String error = String.Format(CultureInfo.InvariantCulture, "User {0} requested page {1}", ((Identity)actionContext.HttpContext.User.Identity).Email, actionContext.HttpContext.Request.Path);
                throw new HttpException((int)HttpStatusCode.Forbidden, error);
            }
        }
    }
}

