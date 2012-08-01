using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using StorageMonster.Common;
using StorageMonster.Domain;
using StorageMonster.Plugin;
using StorageMonster.Services;
using StorageMonster.Web.Models.StorageAccount;
using StorageMonster.Web.Services;
using StorageMonster.Web.Services.ActionAnnotations;
using StorageMonster.Web.Services.Security;
using ValidationResources = StorageMonster.Web.Properties.ValidationResources;
using Common.Logging;
using StorageMonster.Web.Models;

namespace StorageMonster.Web.Controllers
{
    public sealed class StorageAccountController : BaseController
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StorageAccountController));

        public const string StoragePluginsDropDownListCacheKey = "Web.StoragePluginsDropDownListKey";

        private readonly IStoragePluginsService _storagePluginsService;
        private readonly IStorageAccountService _storageAccountService;
        private readonly ICacheService _cacheService;
        public StorageAccountController(IStoragePluginsService storagePluginsService, IStorageAccountService storageAccountService, ICacheService cacheService)
        {
            _storagePluginsService = storagePluginsService;
            _storageAccountService = storageAccountService;
            _cacheService = cacheService;
        }

        [MonsterAuthorize(MonsterRoleProvider.RoleUser, MonsterRoleProvider.RoleAdmin)]
        [MenuActivatorAttribute(MenuActivator.ActivationTypeEnum.ListStorageAccounts)]
        public ActionResult Edit(int id)
        {
            StorageAccount account = _storageAccountService.Load(id);
            if (account == null)
            {
                ModelState.AddModelError("notfound", ValidationResources.StorageAccountNotFoundError);
                return View();
            }

            Identity identity = (Identity)HttpContext.User.Identity;
            if (identity.UserId != account.UserId)
            {
                ModelState.AddModelError("forbidden", ValidationResources.NoPermissionsError);
                return View();
            }

            IStoragePlugin plugin = _storagePluginsService.GetStoragePlugin(account.StoragePluginId);

            if (plugin == null)
            {
                ModelState.AddModelError("storage_plugin", ValidationResources.StoragePluginNotFoundError);
                return View();
            }
            
            ViewData.Add(Constants.StorageAccountIdFormKey, account.Id);
            ViewData.Add(Constants.StampFormKey, account.Stamp.ToBinary());
            ViewData.Add(Constants.StorageAccountTitleViewDataKey, account.AccountName);
            
            
            object pluginSettingsModel = plugin.GetAccountConfigurationModel(account.Id);          

            
            return View(pluginSettingsModel);
        }

        [MonsterAuthorize(MonsterRoleProvider.RoleUser, MonsterRoleProvider.RoleAdmin)]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken(Salt = Constants.Salt_StorageAccount_Edit)]
        [HandleError(ExceptionType = typeof(HttpAntiForgeryException), View = "Forbidden")]
        public ActionResult Edit([ModelBinder(typeof(StorageAccountSettingsModelBinder))]object model)
        {            
            if (ModelState.IsValid)
            {
                int storageAccountId;
                long stamp;

                if (!int.TryParse(Request.Form[Constants.StorageAccountIdFormKey], NumberStyles.Integer, CultureInfo.InvariantCulture, out storageAccountId))
                {
                    ModelState.AddModelError("storage_account_id", ValidationResources.StorageAccountNotFoundError);
                    return View();
                }

                if (!long.TryParse(Request.Form[Constants.StampFormKey], NumberStyles.Integer, CultureInfo.InvariantCulture, out stamp))
                {
                    ModelState.AddModelError("storage_account_id", ValidationResources.BadRequestError);
                    return View();
                }

                //account and plugin should be valid, they were checked by model binder
                StorageAccount account = _storageAccountService.Load(storageAccountId);

                Identity identity = (Identity)HttpContext.User.Identity;
                if (identity.UserId != account.UserId)
                {
                    ModelState.AddModelError("forbidden", ValidationResources.NoPermissionsError);
                    return View();
                }

                IStoragePlugin storagePlugin = _storagePluginsService.GetStoragePlugin(account.StoragePluginId);

                try
                {
                    storagePlugin.ApplyConfiguration(storageAccountId, DateTime.FromBinary(stamp), model);
                    account = _storageAccountService.Load(storageAccountId);
                    ViewData.Add(Constants.StorageAccountIdFormKey, account.Id);
                    ViewData.Add(Constants.StampFormKey, account.Stamp.ToBinary());
                    ViewData.Add(Constants.StorageAccountTitleViewDataKey, account.AccountName);
                    return View(model);
                }
                catch(StaleObjectException)
                {
                    ModelState.AddModelError("storage_account_id", ValidationResources.StorageAccountStalled);
                }
                catch(ObjectNotExistsException)
                {
                    ModelState.AddModelError("storage_account_id", ValidationResources.StorageAccountNotFoundError);
                }
            }
            return View();
        }

        [MonsterAuthorize(MonsterRoleProvider.RoleUser, MonsterRoleProvider.RoleAdmin)]
        [MenuActivatorAttribute(MenuActivator.ActivationTypeEnum.ListStorageAccounts)]
        public ActionResult Add()
        {
            IEnumerable<SelectListItem> storagePlugins = GetSupportedStoragePlugins();
            var model = new StorageAccountModel
                {
                    StoragePlugins = storagePlugins
                };

            return View(model);
        }

        [MonsterAuthorize(MonsterRoleProvider.RoleUser, MonsterRoleProvider.RoleAdmin)]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken(Salt = Constants.Salt_StorageAccount_Add)]
        [HandleError(ExceptionType = typeof(HttpAntiForgeryException), View = "Forbidden")]
        public ActionResult Add(StorageAccountModel storageAccountModel)
        {
            if (storageAccountModel == null)
            {
                storageAccountModel = new StorageAccountModel();
                ModelState.AddModelError("empty_model", ValidationResources.AccountInvalidError);
            }
            IEnumerable<SelectListItem> storagePlugins = GetSupportedStoragePlugins();
            storageAccountModel.StoragePlugins = storagePlugins;

            if (ModelState.IsValid)
            {
                StorageAccount account = new StorageAccount
                    {
                        AccountName = storageAccountModel.AccountName,
                        StoragePluginId = storageAccountModel.PluginId,
                        UserId = ((Identity) System.Web.HttpContext.Current.User.Identity).UserId
                    };
                StorageAccountCreationResult result = _storageAccountService.CreateStorageAccount(account);
                switch (result)
                {
                    case StorageAccountCreationResult.PluginNotFound:
                        ModelState.AddModelError("storage_plugin_not_found", ValidationResources.StoragePluginNotFoundError);
                        break;
                    case StorageAccountCreationResult.AccountExists:
                        ModelState.AddModelError("storage_account_exists", ValidationResources.AccountExistsError);
                        break;
                    case StorageAccountCreationResult.Success:
                        return RedirectToAction("Edit", new {account.Id});
                }
            }

            return View(storageAccountModel);
        }


        [MonsterAuthorize(MonsterRoleProvider.RoleUser, MonsterRoleProvider.RoleAdmin)]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken(Salt = Constants.Salt_StorageAccount_GetFolder)]
        [StorageAccountMenuActivatorAttribute("id")]
        [HandleError(ExceptionType = typeof(HttpAntiForgeryException), View = "Forbidden")]
        public ActionResult GetFolder(int id, string path)
        {
            StorageAccount account = _storageAccountService.Load(id);
            if (account == null)
            {
                ModelState.AddModelError("notfound", ValidationResources.StorageAccountNotFoundError);
                return View();
            }
            Identity identity = (Identity)HttpContext.User.Identity;
            if (identity.UserId != account.UserId)
            {
                ModelState.AddModelError("forbidden", ValidationResources.NoPermissionsError);
                return View();
            }
            IStoragePlugin storagePlugin = _storagePluginsService.GetStoragePlugin(account.StoragePluginId);
            if (storagePlugin == null)
            {
                ModelState.AddModelError("storage_plugin", ValidationResources.StoragePluginNotFoundError);
                return View();
            }

            StorageQueryResult model = null;
            try
            {
                model = storagePlugin.QueryStorage(id, path);
                int errorCounter = 0;
                foreach (var error in model.Errors)                                    
                    ModelState.AddModelError(string.Format(CultureInfo.InvariantCulture,"error_{0}",++errorCounter), error);                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("storage_plugin", ValidationResources.PluginError);
                Logger.Error("Storage plugin error", ex);
                return View();
            }           
            return View(model);
        }

        private IEnumerable<SelectListItem> GetSupportedStoragePlugins()
        {
            return _cacheService.Get(StoragePluginsDropDownListCacheKey, () =>
                _storagePluginsService.GetAvailableStoragePlugins().Select(x => new SelectListItem
                {
                    Text = _storagePluginsService.GetStoragePlugin(x.Id).Name,
                    Value = x.Id.ToString(CultureInfo.InvariantCulture),
                    Selected = false
                }).ToList() /*override lazy init*/);
        }
    }
}
