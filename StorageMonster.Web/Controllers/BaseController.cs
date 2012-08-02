using System;
using System.Web.Mvc;
using StorageMonster.Web.Services.Security;

namespace StorageMonster.Web.Controllers
{
#warning do smth with cache
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")] //disable cache for localization
    public abstract class BaseController : Controller
    {
        public ActionResult Forbidden()
        {
#warning master page?
            return View();
        }

        public ActionResult BadRequest()
        {
#warning master page?
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!(filterContext.HttpContext.User.Identity is Identity))
            {
                throw new InvalidOperationException("Storage monster custom identity is supported only.");
            }
        } 
    }
}
