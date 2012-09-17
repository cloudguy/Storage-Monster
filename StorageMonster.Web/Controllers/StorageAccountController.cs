using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Logging;
using StorageMonster.Common;
using StorageMonster.Domain;
using StorageMonster.Plugin;
using StorageMonster.Services;
using StorageMonster.Web.Models;
using StorageMonster.Web.Models.StorageAccount;
using StorageMonster.Web.Services;
using StorageMonster.Web.Services.ActionAnnotations;
using StorageMonster.Web.Services.Extensions;
using StorageMonster.Web.Services.Security;
using ValidationResources = StorageMonster.Web.Properties.ValidationResources;
using StorageMonster.Web.Properties;
using StorageMonster.Web.Services.ActionResults;

namespace StorageMonster.Web.Controllers
{
    public sealed class StorageAccountController : BaseController
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StorageAccountController));

        private const string StoragePluginsDropDownListCacheKey = "Web.StoragePluginsDropDownListKey";

        private readonly IStoragePluginsService _storagePluginsService;
        private readonly IStorageAccountService _storageAccountService;
        private readonly ICacheService _cacheService;

        public StorageAccountController(IStoragePluginsService storagePluginsService, IStorageAccountService storageAccountService, ICacheService cacheService)
        {
            _storagePluginsService = storagePluginsService;
            _storageAccountService = storageAccountService;
            _cacheService = cacheService;
        }

        [MonsterAuthorize(Constants.RoleUser, Constants.RoleAdmin)]
        [MenuActivatorAttribute(MenuActivator.ActivationTypeEnum.StorageAccountsSettings)]
        public ActionResult Edit(int id)
        {
            StorageAccount account = _storageAccountService.Load(id);
            if (account == null)
            {
                ModelState.AddModelError("notfound", ValidationResources.StorageAccountNotFoundError);
                return View();
            }

            Identity identity = User.Identity;
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

        [MonsterAuthorize(Constants.RoleUser, Constants.RoleAdmin)]
        [MenuActivatorAttribute(MenuActivator.ActivationTypeEnum.StorageAccountsSettings)]
        public ActionResult AskDelete(int id, string returnUrl)
        {
            Identity identity = User.Identity;

            StorageAccount storageAccount = _storageAccountService.Load(id);
            if (storageAccount == null)
            {
                ModelState.AddModelError("id", ValidationResources.StorageAccountNotFoundError);
                return View();
            }

            if (storageAccount.UserId != identity.UserId)
            {
                ModelState.AddModelError("forbidden", ValidationResources.NoPermissionsError);
                return View();
            }

            AskDeleteModel model = new AskDeleteModel
            {
                StorageAccountId = id,
                StorageAccountName = storageAccount.AccountName,
                ReturnUrl = returnUrl ?? Url.Action("StorageAccounts", "User", new { Id = identity.UserId })
            };
            return View(model);
        }


        [MonsterAuthorize(Constants.RoleUser, Constants.RoleAdmin)]
        [MenuActivatorAttribute(MenuActivator.ActivationTypeEnum.StorageAccountsSettings)]
        [MonsterValidateAntiForgeryToken(Salt = Constants.Salt_StorageAccount_Delete)]      
        public ActionResult Delete(AskDeleteModel model)
        {
            if (ModelState.IsValid)
            {
                StorageAccount storageAccount = _storageAccountService.Load(model.StorageAccountId);
                if (storageAccount == null)
                {
                    ModelState.AddModelError("id", ValidationResources.StorageAccountNotFoundError);
                    return View("AskDelete");
                }

                Identity identity = User.Identity;

                if (storageAccount.UserId != identity.UserId)
                {
                    ModelState.AddModelError("forbidden", ValidationResources.NoPermissionsError);
                    return View("AskDelete");
                }

                _storageAccountService.DeleteStorageAccount(storageAccount.Id);

                TempData.AddRequestSuccessMessage(string.Format(CultureInfo.CurrentCulture, SuccessMessagesResources.StorageAccountDeletedFormat, storageAccount.AccountName));
                               
                if (Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);

                return RedirectToAction("StorageAccounts", "User", new { Id = identity.UserId });

            }
            return View("AskDelete", model);            
        }

        [MonsterAuthorize(Constants.RoleUser, Constants.RoleAdmin)]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [MonsterValidateAntiForgeryToken(Salt = Constants.Salt_StorageAccount_Edit)]       
        [MenuActivatorAttribute(MenuActivator.ActivationTypeEnum.StorageAccountsSettings)]
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
                    TempData.AddRequestSuccessMessage(string.Format(CultureInfo.CurrentCulture, SuccessMessagesResources.StorageAccountUpdatedFormat, account.AccountName));
                    return RedirectToAction("Edit");                  
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
            return View(model);
        }

        [MonsterAuthorize(Constants.RoleUser, Constants.RoleAdmin)]
        [MenuActivatorAttribute(MenuActivator.ActivationTypeEnum.StorageAccountsSettings)]
        public ActionResult Add()
        {
            IEnumerable<SelectListItem> storagePlugins = GetSupportedStoragePlugins();
            var model = new StorageAccountModel
                {
                    StoragePlugins = storagePlugins
                };

            return View(model);
        }

        [MonsterAuthorize(Constants.RoleUser, Constants.RoleAdmin)]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [MonsterValidateAntiForgeryToken(Salt = Constants.Salt_StorageAccount_Add)]      
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
                        UserId = User.Identity.UserId
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

        public ActionResult GetFile(string url, int id)
        {
            string redirectUrl;
            if (Request.UrlReferrer != null && Url.IsLocalUrl(Request.UrlReferrer.ToString()))
                redirectUrl = Request.UrlReferrer.PathAndQuery;
            else
                redirectUrl = Url.Action("Index", "Home");
            StorageAccount account = _storageAccountService.Load(id);
            if (account == null)
            {
                TempData.AddRequestErrorMessage(ValidationResources.StorageAccountNotFoundError);
                return Redirect(redirectUrl);
            }

            if (string.IsNullOrEmpty(url))
            {
                TempData.AddRequestErrorMessage(ValidationResources.InvalidFileLocation);
                return Redirect(redirectUrl);
            }

            if (User == null || User.Identity == null)
            {
                TempData.AddRequestErrorMessage(ValidationResources.NoPermissionsToDownload);
                return Redirect(redirectUrl);
            }

            if (account.UserId != User.Identity.UserId)
            {
                TempData.AddRequestErrorMessage(ValidationResources.NoPermissionsToDownload);
                return Redirect(redirectUrl);
            }
            try
            {
                var streamResult = _storagePluginsService.DownloadFile(account, url);
                return new BufferLessFileResult(streamResult.FileStream, streamResult.FileName);
            }
            catch (StoragePluginNotFoundException)
            {
                TempData.AddRequestErrorMessage(ValidationResources.StoragePluginNotFoundError);
            }
            catch (PluginException ex)
            {
                TempData.AddRequestErrorMessage(GetErrorMessage(ex.ErrorCode));
                if (ex.ErrorCode == PluginErrorCodes.PluginError)
                    Logger.Error("Storage plugin error", ex);               
            }
            catch (Exception ex)
            {
                TempData.AddRequestErrorMessage(ErrorResources.PluginError);              
                Logger.Error("Storage plugin error", ex);               
            }

            return Redirect(redirectUrl);
        }


        [MonsterAuthorize(Constants.RoleUser, Constants.RoleAdmin)]        
        [ValidateInput(false)]
        [StorageAccountMenuActivator("id")]       
        public ActionResult GetFolder(int id, string path)
        {
            FolderModel model = null;

            var resultAction = Condition()
                        .DoIfNotAjax(() => View(model))
                        .DoIfAjax(() => Json(new AjaxResult
                        {
                            MainPanelHtml = this.RenderViewToString("~/Views/StorageAccount/Controls/StorageAccountFolderControl.ascx", model)
                        }, JsonRequestBehavior.AllowGet));

            StorageAccount account = _storageAccountService.Load(id);
            if (account == null)
            {
                ModelState.AddModelError("notfound", ValidationResources.StorageAccountNotFoundError);
                return resultAction;
            }
            Identity identity = (Identity)HttpContext.User.Identity;
            if (identity.UserId != account.UserId)
            {
                ModelState.AddModelError("forbidden", ValidationResources.NoPermissionsError);
                return resultAction;
            }
#warning call service not plugin dirrectly
            IStoragePlugin storagePlugin = _storagePluginsService.GetStoragePlugin(account.StoragePluginId);
            if (storagePlugin == null)
            {
                ModelState.AddModelError("storage_plugin", ValidationResources.StoragePluginNotFoundError);
                return resultAction;
            }

            StorageFolderResult queryModel = null;

            try
            {
                queryModel = storagePlugin.QueryStorage(id, path);                
            }
            catch (PluginException ex)
            {
                ModelState.AddModelError("storage_plugin", GetErrorMessage(ex.ErrorCode));
                if (ex.ErrorCode == PluginErrorCodes.PluginError)
                    Logger.Error("Storage plugin error", ex);
                return resultAction;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("storage_plugin", ErrorResources.PluginError);
                Logger.Error("Storage plugin error", ex);
                return resultAction;
            }
           
            model = new FolderModel()
            {
                Content = queryModel,                
                StorageAccount = account
            };

            return resultAction;
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

        private static string GetErrorMessage(PluginErrorCodes errorCode)
        {
            switch (errorCode)
            {
                case PluginErrorCodes.CouldNotContactStorageService:
                    return ErrorResources.CouldNotContactStorageService;
                case PluginErrorCodes.InvalidFileOrDirectoryName:
                    return ErrorResources.InvalidFileOrDirectoryName;
                case PluginErrorCodes.FileNotFound:
                    return ErrorResources.FileNotFound;
                case PluginErrorCodes.InvalidCredentialsOrConfiguration:
                    return ErrorResources.InvalidCredentialsOrConfiguration;
                case PluginErrorCodes.CouldNotRetrieveDirectoryList:
                    return ErrorResources.CouldNotRetrieveDirectoryList;
                case PluginErrorCodes.CreateOperationFailed:
                    return ErrorResources.CreateOperationFailed;
                case PluginErrorCodes.LimitExceeded:
                    return ErrorResources.LimitExceeded;
                case PluginErrorCodes.InsufficientDiskSpace:
                    return ErrorResources.InsufficientDiskSpace;
                case PluginErrorCodes.TransferAbortedManually:
                    return ErrorResources.TransferAbortedManually;
                default:
                    return ErrorResources.PluginError;
            }            
        }
    }
}
