using System;
using System.Globalization;
using System.Web.Mvc;
using StorageMonster.Services;
using StorageMonster.Web.Services.ActionAnnotations;
using StorageMonster.Web.Services.Configuration;
using StorageMonster.Web.Services.Security;
using StorageMonster.Web.Services.ActionResults;

namespace StorageMonster.Web.Controllers
{
#warning do smth with cache
    [TempDataTransfer]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")] //disable cache for localization
    public abstract class BaseController : Controller
    {
        protected ConditionalResult Condition()
        {
            return new ConditionalResult();
        }
 
        private const string DefaultUrlScheme = "http";
        public ActionResult Forbidden()
        {
#warning check urlreferer
            return View();
        }

        public ActionResult BadRequest()
        {
#warning check urlreferer
            return View();
        }
        public ActionResult NotFound()
        {
#warning check urlreferer
            return View();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!(filterContext.HttpContext.User.Identity is Identity))
            {
                throw new InvalidOperationException("Storage monster custom identity is supported only.");
            }
        } 

        protected string FullUrlForAction(string action, string controller, object routeValues)
        {
            IWebConfiguration webConfig = IocContainer.Instance.Resolve<IWebConfiguration>();
            if (webConfig.AutoDetectSiteUrl)
            {
                string scheme;
                if (Request == null || Request.Url == null)
                    scheme = DefaultUrlScheme;
                else
                    scheme = Request.Url.Scheme;
                return Url.Action(action, controller, routeValues, scheme);
            }

            string relativeUrl = Url.Action(action, controller, routeValues).TrimStart(new []{'~', '/'});
            return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", webConfig.SiteUrl.TrimEnd('/'), relativeUrl);
        }

        public new Principal User
        {
            get { return (Principal)base.User; }
        }

        protected string BaseSiteUrl()
        {
            IWebConfiguration webConfig = IocContainer.Instance.Resolve<IWebConfiguration>();
            if (webConfig.AutoDetectSiteUrl)
            {
                string scheme;
                if (Request == null || Request.Url == null)
                    scheme = DefaultUrlScheme;
                else
                    scheme = Request.Url.Scheme;
                return Url.Action("Index", "Home", null, scheme);
            }
            return webConfig.SiteUrl;
        }
    }
}
