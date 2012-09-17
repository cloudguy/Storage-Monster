using System;
using System.Web.Mvc;
using Common.Logging;
using StorageMonster.Web.Services.Security;
using StorageMonster.Web.Properties;

namespace StorageMonster.Web.Services.ActionAnnotations
{
#warning rewrite antiforgery http://stackoverflow.com/a/2206819/1540039
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class MonsterValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        private string _salt;
        public string Salt
        {
            get { return this._salt; }
            set { this._salt = value ?? string.Empty; _validator.Salt = _salt; }
        }

        //ValidateAntiForgeryTokenAttribute class is sealed,
        //so this class is just a wrapper aginst it
        private readonly ValidateAntiForgeryTokenAttribute _validator = new ValidateAntiForgeryTokenAttribute();      
        

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                _validator.OnAuthorization(filterContext);
            }
            catch (HttpAntiForgeryException antiForgeryException)
            {
                ForbiddenRequestsLogger.LogRequest(filterContext.HttpContext.Request, antiForgeryException);
                filterContext.Controller.ViewData.ModelState.AddModelError("forbidden", ValidationResources.AntiforgeryMismath);
            }
        }
    }

}