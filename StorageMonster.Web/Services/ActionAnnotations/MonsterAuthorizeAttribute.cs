using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StorageMonster.Domain;
using StorageMonster.Web.Models;
using StorageMonster.Web.Models.Account;
using StorageMonster.Web.Services.Extensions;
using StorageMonster.Web.Services.Security;

namespace StorageMonster.Web.Services.ActionAnnotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class MonsterAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly UserRole _roleMask;

        public MonsterAuthorizeAttribute(UserRole roleMask)
        {
            _roleMask = roleMask;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.HttpContext.User.Identity.IsAuthenticated)
            {
                
                if (!actionContext.HttpContext.Request.IsAjaxRequest())
                {
                    actionContext.Result = AuthorizationHelper.GetAuthAjaxResult(actionContext, false);
                    return;
                }
                AuthorizationHelper.RedirectToLogon(HttpContext.Current.Request, HttpContext.Current.Response);
                return;
            }
            
#warning rewrite
            /*string role = _roles.FirstOrDefault(r => actionContext.HttpContext.User.IsInRole(r));

            if (role == null)
            {
                String error = String.Format(CultureInfo.InvariantCulture, "User {0} requested page {1}", ((Identity) actionContext.HttpContext.User.Identity).Email, actionContext.HttpContext.Request.Path);
#warning add route
                throw new HttpException((int) HttpStatusCode.Forbidden, error);
            }*/
        }
    }
}