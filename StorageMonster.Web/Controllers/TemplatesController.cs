using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Controllers
{
    public class TemplatesController : Controller
    {
        public ActionResult Get(string templateName)
        {
            return View();
        }

    }
}
