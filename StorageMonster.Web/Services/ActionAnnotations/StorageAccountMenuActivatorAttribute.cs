using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StorageMonster.Web.Models;

namespace StorageMonster.Web.Services.ActionAnnotations
{
#warning maybe remove?
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class StorageAccountMenuActivatorAttribute : ActionFilterAttribute
    {
        protected string StorageAccountformKey { get; set; }

        public StorageAccountMenuActivatorAttribute(string storageAccountformKey)
        {
            StorageAccountformKey = storageAccountformKey;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string formValue = filterContext.HttpContext.Request.Form[StorageAccountformKey];
            int accountId = -1;
            int.TryParse(formValue, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out accountId);

            if (filterContext != null && filterContext.Controller != null)
                filterContext.Controller.ViewData.Add(Constants.MenuActivatorViewDataKey, new MenuActivator { ActivationType = MenuActivator.ActivationTypeEnum.StorageAccount, StorageAccountId = accountId });
        }
    }
}