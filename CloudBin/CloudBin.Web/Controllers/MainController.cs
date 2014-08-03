using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Common.Logging;
using CloudBin.Data;

namespace CloudBin.Web.Controllers
{
    public class MainController : Controller
    {
        //private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            var repo = DependencyResolver.Current.GetService<IStoragePluginDescriptorRepository>();
            var res = repo.Read(1);
            //Log.Error("zzzz");
            ViewData["Message"] = "Welcome to ASP.NET MVC on Mono!";
            return View();
        }

        public ActionResult Test()
        {
            //Log.Error("zzzz");
            ViewData["Message"] = "Welcome to ASP.NET MVC on Mono!";
            return View("Index");
        }
    }
}
