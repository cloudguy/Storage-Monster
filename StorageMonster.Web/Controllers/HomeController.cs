using System;
using System.Linq;
using System.Web.Mvc;
using StorageMonster.DB.Repositories;
using StorageMonster.Services;
using StorageMonster.Services.Security;
using StorageMonster.Web.Models;
using StorageMonster.Web.Services;


namespace StorageMonster.Web.Controllers
{
    public class HomeController : BaseController
    {
        protected IAccountService AccountService;
        protected IStorageService StorageService;
        public HomeController(IAccountService accountService, IStorageService storageService)
        {
            AccountService = accountService;
            StorageService = storageService;
        }


        [MonsterAuthorize(new[] { MonsterRoleProvider.RoleUser, MonsterRoleProvider.RoleAdmin })]
        public ActionResult Index()
        {
            int userId = ((Identity) HttpContext.User.Identity).UserId;
            var accounts = AccountService.GetActiveAccounts(userId);

            UserMenuModel model = new UserMenuModel
                {
                    Accounts = accounts.Select(a => new UserMenuModel.AccountItem()
                        {
                            AccountId = a.Object1.Id,
                            AccountLogin = a.Object1.AccountLogin,
                            AccountServer = a.Object1.AccountServer,
                            StorageId = a.Object1.StorageId,
                            StorageName = StorageService.GetStoragePlugin(a.Object2.Id).Name
                        }),

                    UserId = userId
                };

            return View(model);
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Forbidden()
        {
            return View();
        }

        //public A
       

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
