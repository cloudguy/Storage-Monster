using StorageMonster.Web.Properties;
using StorageMonster.Web.Services.Security;
using System;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.ActionAnnotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class MonsterValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        private string _salt;
        public string Salt
        {
            get { return _salt; }
            set { _salt = value ?? string.Empty; _validator.Salt = _salt; }
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