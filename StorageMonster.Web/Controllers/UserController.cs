using System;
using System.Web.Mvc;
using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Web.Models;
using StorageMonster.Web.Models.User;
using StorageMonster.Web.Properties;
using StorageMonster.Web.Services.ActionAnnotations;
using StorageMonster.Web.Services.Security;
using System.Web;
using System.Net;
using StorageMonster.Web.Services.Extensions;

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

        [MonsterAuthorize(Constants.RoleUser, Constants.RoleAdmin)]
        [MenuActivatorAttribute(MenuActivator.ActivationTypeEnum.StorageAccountsSettings)]
        public ActionResult StorageAccounts(int? id)
        {
            if (id == null)
                throw new HttpException((int)HttpStatusCode.NotFound, string.Empty);
            UserAccountsModel userAccountsModel = null;

            var resultAction = Condition()
                        .DoIfNotAjax(() => View(userAccountsModel))
                        .DoIfAjax(() => Json(new AjaxResult
                        {
                            MainPanelHtml = this.RenderViewToString("~/Views/User/Controls/StorageAccountsControl.ascx", userAccountsModel)
                        }, JsonRequestBehavior.AllowGet));


            Identity identity = User.Identity;          

            if (identity.UserId != id && !HttpContext.User.IsInRole(Constants.RoleAdmin))
            {
                ModelState.AddModelError("forbidden", ValidationResources.NoPermissionsError);
                return resultAction;
            }

            User targetUser = _userService.Load(id.Value);
            if (targetUser == null)
            {
                ModelState.AddModelError("usernotfound", ValidationResources.UserNotFoundError);
                return resultAction;
            }
            userAccountsModel = new UserAccountsModel();
            userAccountsModel.CanAddAcounts = identity.UserId == id;
            userAccountsModel.CanDeleteAcounts = identity.UserId == id;
            userAccountsModel.CanEditAcounts = identity.UserId == id;
            StorageAccountsCollection targetAccountsCollection = new StorageAccountsCollection().Init(_accountService, _storageService, id.Value);
            userAccountsModel.AccountsCollection = targetAccountsCollection;
            return resultAction;
        }
    }
}