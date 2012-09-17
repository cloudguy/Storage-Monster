using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StorageMonster.Web.Models;

namespace StorageMonster.Web.Services.ActionAnnotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class StorageAccountMenuActivatorAttribute : ActionFilterAttribute
    {
        private readonly string _storageAccountformKey;

        public StorageAccountMenuActivatorAttribute(string storageAccountformKey)
        {
            _storageAccountformKey = storageAccountformKey;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string formValue = filterContext.RouteData.Values[_storageAccountformKey] as string;
            int accountId = -1;
            int.TryParse(formValue, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out accountId);

            if (filterContext != null && filterContext.Controller != null)
                filterContext.Controller.ViewData.Add(Constants.MenuActivatorViewDataKey, new MenuActivator { ActivationType = MenuActivator.ActivationTypeEnum.StorageAccount, StorageAccountId = accountId });
        }
    }
}