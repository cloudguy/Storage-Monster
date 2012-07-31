using System;
using System.Web.Mvc;
using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Web.Models;
using StorageMonster.Web.Models.User;
using StorageMonster.Web.Properties;
using StorageMonster.Web.Services.ActionAnnotations;
using StorageMonster.Web.Services.Security;

namespace StorageMonster.Web.Controllers
{
    public sealed class UserController : BaseController
    {
        private readonly IStorageAccountService _accountService;
        private readonly IStoragePluginsService _storageService;
        private readonly IUserService _userService;

        public UserController(IStorageAccountService accountService, IStoragePluginsService storageService, IUserService userService)
        {
            _accountService = accountService;
            _storageService = storageService;
            _userService = userService;
        }

        [MonsterAuthorize(MonsterRoleProvider.RoleUser, MonsterRoleProvider.RoleAdmin)]
        [MenuActivatorAttribute(MenuActivator.ActivationTypeEnum.ListStorageAccounts)]
        public ActionResult StorageAccounts(int Id)
        {
            Identity identity = (Identity) HttpContext.User.Identity;
            UserAccountsModel userAccountsModel = new UserAccountsModel();

            if (identity.UserId != Id && !HttpContext.User.IsInRole(MonsterRoleProvider.RoleAdmin))
            {
                ModelState.AddModelError("forbidden", ValidationResources.NoPermissionsError);
#warning activate admin menu
#warning check rights to edit and delete
                return View(userAccountsModel);
            }

            User targetUser = _userService.Load(Id);
            if (targetUser == null)
            {
                ModelState.AddModelError("usernotfound", ValidationResources.UserNotFoundError);
                return View(userAccountsModel);
            }
            StorageAccountsCollection targetAccountsCollection = new StorageAccountsCollection().Init(_accountService, _storageService, Id);
            userAccountsModel.AccountsCollection = targetAccountsCollection;
            return View(userAccountsModel);
        }
    }
}