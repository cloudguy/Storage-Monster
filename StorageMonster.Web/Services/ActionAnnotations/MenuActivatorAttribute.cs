using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StorageMonster.Web.Models;

namespace StorageMonster.Web.Services.ActionAnnotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class MenuActivatorAttribute : ActionFilterAttribute
    {
        protected MenuActivator.ActivationTypeEnum ActivationType { get; set; }

        public MenuActivatorAttribute(MenuActivator.ActivationTypeEnum activationType)
        {
            ActivationType = activationType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {        
           if (filterContext != null && filterContext.Controller != null)
                filterContext.Controller.ViewData.Add(Constants.MenuActivatorViewDataKey, new MenuActivator { ActivationType = ActivationType });           
        }
    }
}

