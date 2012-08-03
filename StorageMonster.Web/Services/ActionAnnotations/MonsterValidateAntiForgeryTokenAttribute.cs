using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Logging;

namespace StorageMonster.Web.Services.ActionAnnotations
{
#warning put it everywhere
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class MonsterValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        private static readonly ILog _forbiddenLogger = LogManager.GetLogger("ForbiddenRequests");

        private string _salt;
        public string Salt
        {
            get { return this._salt ?? string.Empty; }
            set { this._salt = value; }
        }
        ValidateAntiForgeryTokenAttribute validator = new ValidateAntiForgeryTokenAttribute();
        public MonsterValidateAntiForgeryTokenAttribute()
        {
            validator.Salt = Salt;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                validator.OnAuthorization(filterContext);
            }
            catch (HttpAntiForgeryException antiForgeryException)
            {
#warning log path and ip
                _forbiddenLogger.Warn(antiForgeryException);
#warning localization
                filterContext.Controller.ViewData.ModelState.AddModelError("forbidden", "Anti forgery, епта!");
            }
        }
    }

}