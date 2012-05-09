using System;
using System.Web.Mvc;
using StorageMonster.Services.Security;
using StorageMonster.Web.Services;


namespace StorageMonster.Web.Controllers
{
    public class HomeController : BaseController
    {
        [MonsterAuthorize(new[] { MonsterRoleProvider.RoleUser, MonsterRoleProvider.RoleAdmin })]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Forbidden()
        {
            return View();
        }

       

#if DEBUG

        [MonsterAuthorize(new[] { MonsterRoleProvider.RoleUser })]
        public ActionResult TestUser()
        {
            return new EmptyResult();
        }

        [MonsterAuthorize(new[] { MonsterRoleProvider.RoleAdmin })]
        public ActionResult TestAdmin()
        {
            return new EmptyResult();
        }

        public ActionResult CrashTest()
        {
            throw new Exception("Crash test");
        }
#endif
    }
}
