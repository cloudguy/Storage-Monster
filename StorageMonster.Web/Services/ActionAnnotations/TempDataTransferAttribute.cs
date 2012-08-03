using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StorageMonster.Web.Services.Extensions;

namespace StorageMonster.Web.Services.ActionAnnotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class TempDataTransferAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var successMsgs = actionContext.Controller.TempData.GetRequestSuccessMessages();
            if (successMsgs != null)
            {
                foreach (var msg in successMsgs)
                    actionContext.Controller.ViewData.AddRequestSuccessMessage(msg);
            }

            var errorMsgs = actionContext.Controller.TempData.GetRequestErrorMessages();
            if (errorMsgs != null)
            {
                foreach (var msg in errorMsgs)
                    actionContext.Controller.ViewData.ModelState.AddModelError(msg.Key, msg.Value);
            }
        }
    }
}
