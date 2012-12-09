using System;
using System.Web.Mvc;
using StorageMonster.Domain;
using StorageMonster.Web.Services.ActionAnnotations;

namespace StorageMonster.Web.Controllers
{
    public class HomeController : BaseController
    {
        [MonsterAuthorize(UserRoles.RoleUser, UserRoles.RoleUser)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CrashTest()
        {
            throw new Exception();
        }
    }
}
