using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppLimit.CloudComputing.SharpBox;
using AppLimit.CloudComputing.SharpBox.StorageProvider;
using AppLimit.CloudComputing.SharpBox.StorageProvider.WebDav;
using StorageMonster.Database;
using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Services.Security;
using StorageMonster.Plugin.StorageQuery;
using StorageMonster.Common;
using AppLimit.CloudComputing.SharpBox.Exceptions;
using StorageMonster.Utilities;
using System.Globalization;

namespace StorageMonster.Plugin.WebDav
{
    public class WebDavPlugin : IStoragePlugin
    {
        public virtual string Name { get { return "WebDav"; } }

        protected IStorageAccountService AccountService { get; set; }
        protected ICryptoService CryptoService { get; set; }
        protected ISecurityConfiguration SecurityConfiguration { get; set; }

        public WebDavPlugin(IStorageAccountService accountService, 
            ICryptoService cryptoService,
            ISecurityConfiguration securityConfiguration)
        {
            AccountService = accountService;
            CryptoService = cryptoService;
            SecurityConfiguration = securityConfiguration;
        }

        public virtual object GetAccountConfigurationModel(int accountId)
		{
		    IEnumerable<StorageAccountSetting> settings = AccountService.GetSettingsForStoargeAccount(accountId);
		    var model = new WebDavConfigurationModel
		        {
                    ServerUrl = settings.Where(s => string.Equals("server", s.SettingName, StringComparison.OrdinalIgnoreCase))
                                .Select(s=>s.SettingValue)
                                .FirstOrDefault(),
                    AccountLogin = settings.Where(s => string.Equals("login", s.SettingName, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.SettingValue)
                                .FirstOrDefault(),                    
		        };

            string accountPasswordEncrypted = settings.Where(s => string.Equals("password", s.SettingName, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.SettingValue)
                                .FirstOrDefault();
            if (!string.IsNullOrEmpty(accountPasswordEncrypted))
                model.AccountPassword = CryptoService.DecryptString(accountPasswordEncrypted, SecurityConfiguration.EncryptionKey, SecurityConfiguration.EncryptionSalt);

            return model;
		}

        public virtual object GetAccountConfigurationModel()
        {
            return new WebDavConfigurationModel();
        }

        public virtual void ApplyConfiguration(int accountId, DateTime accountStamp, object configurationModel)
        {
            WebDavConfigurationModel webDavConfigurationModel = configurationModel as WebDavConfigurationModel;
            if (webDavConfigurationModel == null)
                return;

            Dictionary<string, string> settings = new Dictionary<string, string>
                {
                    {"login", webDavConfigurationModel.AccountLogin}, 
                    {"password", CryptoService.EncryptString(webDavConfigurationModel.AccountPassword, SecurityConfiguration.EncryptionKey, SecurityConfiguration.EncryptionSalt)}, 
                    {"server", webDavConfigurationModel.ServerUrl}
                };            
            AccountService.SaveSettings(settings, accountId, accountStamp);
        }

        public StorageQueryResult QueryStorage(int accountId, string path)
        {
            StorageQueryResult result = new StorageQueryResult();

            Action queryAction = new Action(() => 
            {
                WebDavConfigurationModel model = GetAccountConfigurationModel(accountId) as WebDavConfigurationModel;

                model = model.With(m => !string.IsNullOrEmpty(m.ServerUrl))
                    .With(m => !string.IsNullOrEmpty(m.AccountLogin))
                    .With(m => !string.IsNullOrEmpty(m.AccountPassword));

                if (model == null)
                    throw new MonsterFrontException(ValidationResources.StorageAccountNotConfiguredError);

                Uri uri = new Uri(model.ServerUrl);
                ICloudStorageConfiguration config = new WebDavConfiguration(uri);
                GenericNetworkCredentials cred = new GenericNetworkCredentials();
                cred.UserName = model.AccountLogin;
                cred.Password = model.AccountPassword;

                CloudStorage storage = null;
                try
                {
                    storage = new CloudStorage();
                    ICloudStorageAccessToken storageToken = storage.Open(config, cred);
                    if (string.IsNullOrEmpty(path))
                        path = "/";
                    ICloudDirectoryEntry directory = storage.GetFolder(path, true);
                    foreach (var entry in directory)
                    {
                        var dirEntry = entry as ICloudDirectoryEntry;
                        string entryPath = string.Format(CultureInfo.InvariantCulture,"{0}{1}{2}", path, "/", entry.Name);
                        if (dirEntry != null)
                        {
                            result.AddItem(new StorageFolder 
                            { 
                                Name = dirEntry.Name,
                                Path = entryPath,
                                StorageAccountId = accountId
                            });
                            continue;
                        }
                        result.AddItem(new StorageFile
                        {
                            Name = entry.Name,
                            Path = entryPath,
                            StorageAccountId = accountId
                        });
                    }
                }
                finally
                {
                    if (storage != null)
                        storage.Close();
                }

            });

            StorageQueryExecutor.WithAction(queryAction)
                .IfExceptionIs(typeof(MonsterFrontException))
                    .CatchIt((ex) => result.AddError(ex.Message))
                .IfExceptionIs(typeof(UriFormatException))
                    .CatchIt((ex) => result.AddError(ValidationResources.BadServerError))  
                .IfExceptionIs(typeof(SharpBoxException))
                    .CatchIt((ex)=>result.AddError(ValidationResources.CanNotConnectError))
                .Run();

            return result;
        }		
    }
}