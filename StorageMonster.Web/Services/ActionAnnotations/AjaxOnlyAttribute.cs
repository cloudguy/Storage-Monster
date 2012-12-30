using System;
using System.Net;
using System.Web.Mvc;
using StorageMonster.Web.Models;
using StorageMonster.Web.Properties;

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
            bool isAjax = request.IsAjaxRequest();
            bool wrongMethod = _jsonRequestBehavior == JsonRequestBehavior.DenyGet && (string.Compare(request.HttpMethod.Trim(), "GET", StringComparison.OrdinalIgnoreCase) == 0);
            
            if (!isAjax || wrongMethod)
            {
                actionContext.HttpContext.Response.Clear();
                actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;

                if (isAjax)
                {
                    actionContext.Result = new JsonResult
                        {
                            Data = new AjaxErrorModel {Error = ValidationResources.AjaxNotAcceptable},
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            ContentEncoding = System.Text.Encoding.UTF8,
                            ContentType = "application/json",
                        };
                }
                else
                {
                    actionContext.Result = new EmptyResult();
                }
            }
        }
    }
}

