using System;
using System.Net;
using System.Web.Mvc;
using StorageMonster.Web.Models;

namespace StorageMonster.Web.Services.ActionAnnotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        private readonly JsonRequestBehavior _jsonRequestBehavior;

        public AjaxOnlyAttribute(JsonRequestBehavior jsonRequestBehavior)
        {
            _jsonRequestBehavior = jsonRequestBehavior;
        }
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var request = actionContext.HttpContext.Request;
            if (!request.IsAjaxRequest() || (_jsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Compare(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase) == 0))
            {
                actionContext.HttpContext.Response.Clear();
                actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                AjaxErrorModel ajaxErrorModel = new AjaxErrorModel
                    {
                        Error = Properties.ValidationResources.AjaxNotAcceptable
                    };

                actionContext.Result = new JsonResult
                    {
                        Data = ajaxErrorModel,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        ContentEncoding = System.Text.Encoding.UTF8,
                        ContentType = Constants.JsonContentType,
                    };
            }
        }
    }
}

