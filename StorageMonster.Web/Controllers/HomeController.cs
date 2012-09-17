using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Web.Models;
using StorageMonster.Web.Services.ActionAnnotations;
using StorageMonster.Web.Services.Extensions;
using StorageMonster.Web.Services.Security;
using StorageMonster.Web.Properties;
using StorageMonster.Plugin;
using Newtonsoft.Json;
using System.Web.Security;
using System.IO;


namespace StorageMonster.Web.Controllers
{    
    public sealed class HomeController : BaseController
    {
        public HomeController()
        {
        }

        [MonsterAuthorize(Constants.RoleUser, Constants.RoleAdmin)]
        public ActionResult Index()
        {
            //var service = IocContainer.Instance.Resolve<IStoragePluginsService>();
            //var plugin = service.GetStoragePlugin(1);
            //StreamReader  reader= new StreamReader(
            //var s = plugin.GetFileStream("/logsql.txt", 9, null);
            return View();
        }      
      
#warning move to base
        /*
        [AjaxOnly(JsonRequestBehavior.AllowGet)]
        [MonsterAuthorize(Constants.RoleUser, Constants.RoleAdmin)]
        public ActionResult UserMenu()
        {
            Identity identity = (Identity)HttpContext.User.Identity;
            StorageAccountsCollection accountsCollection = new StorageAccountsCollection().Init(AccountService, StorageService, identity.UserId);
            UserMenuModel menuModel = new UserMenuModel
            {
                StorageAccountsCollection = accountsCollection
            };
            string viewContent = this.RenderViewToString("UserMenu", menuModel);
            return Json(new { Menu = viewContent }, JsonRequestBehavior.AllowGet);
        }
        */
 

#if DEBUG

        [MonsterAuthorize(Constants.RoleUser)]
        public ActionResult TestUser()
        {
            return new EmptyResult();
        }


        [MonsterAuthorize(Constants.RoleAdmin)]
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
