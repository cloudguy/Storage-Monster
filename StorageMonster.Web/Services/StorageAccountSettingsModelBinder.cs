using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StorageMonster.Domain;
using StorageMonster.Plugin;
using StorageMonster.Services;
using StorageMonster.Web.Properties;

namespace StorageMonster.Web.Services
{
    public class StorageAccountSettingsModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NameValueCollection form = controllerContext.HttpContext.Request.Form;
            if (!form.AllKeys.Contains(Constants.StorageAccountIdFormKey))
            {
                bindingContext.ModelState.AddModelError("storage_account", ValidationResources.StorageAccountNotFoundError);
                return null;
            }

            string pluginFormValue = form[Constants.StorageAccountIdFormKey];

            int accountId;
            if (!int.TryParse(pluginFormValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out accountId))
            {
                bindingContext.ModelState.AddModelError("storage_account", ValidationResources.StorageAccountNotFoundError);
                return null;
            }

            IStorageAccountService accountService = IocContainer.Instance.Resolve<IStorageAccountService>();
            StorageAccount account = accountService.Load(accountId);
            if (account == null)
            {
                bindingContext.ModelState.AddModelError("storage_account", ValidationResources.StorageAccountNotFoundError);
                return null;
            }

            IStoragePluginsService storageService = IocContainer.Instance.Resolve<IStoragePluginsService>();

            IStoragePlugin storagePlugin = storageService.GetStoragePlugin(account.StoragePluginId);

            if (storagePlugin == null)
            {
                bindingContext.ModelState.AddModelError("storage_plugin", ValidationResources.StoragePluginNotFoundError);
                return null;
            }

            Type modelType = storagePlugin.GetAccountConfigurationModel().GetType();

            if (!String.IsNullOrEmpty(bindingContext.ModelName) && !bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName))
            {
                if (!bindingContext.FallbackToEmptyPrefix)
                    return null;

                bindingContext = new ModelBindingContext
                {
                    ModelMetadata = bindingContext.ModelMetadata,
                    ModelState = bindingContext.ModelState,
                    PropertyFilter = bindingContext.PropertyFilter,
                    ValueProvider = bindingContext.ValueProvider
                };
            }

            //object model = bindingContext.Model ?? CreateModel(controllerContext, bindingContext, bindingContext.ModelType);
            object model = bindingContext.Model ?? CreateModel(controllerContext, bindingContext, modelType);

            bindingContext = new ModelBindingContext
            {
                //ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, bindingContext.ModelType),
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, modelType),
                ModelName = bindingContext.ModelName,
                ModelState = bindingContext.ModelState,
                PropertyFilter = bindingContext.PropertyFilter,
                ValueProvider = bindingContext.ValueProvider
            };

            if (OnModelUpdating(controllerContext, bindingContext))
            {
                foreach (PropertyDescriptor descriptor in GetFilteredModelProperties(controllerContext, bindingContext))
                    BindProperty(controllerContext, bindingContext, descriptor);

                OnModelUpdated(controllerContext, bindingContext);
            }

            return model;
        }
    }
}
