using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Services.Security;

namespace StorageMonster.Plugin.WebDav
{
    public class ConfigurationProvider
    {
        private IStorageAccountService _accountService;
        private ICryptoService _cryptoService;
        private ISecurityConfiguration _securityConfiguration;

        public ConfigurationProvider(IStorageAccountService accountService, ICryptoService cryptoService, ISecurityConfiguration securityConfiguration)
        {
            _accountService = accountService;
            _cryptoService = cryptoService;
            _securityConfiguration = securityConfiguration;
        }
        public object GetAccountConfigurationModel(int accountId)
        {
            IEnumerable<StorageAccountSetting> settings = _accountService.GetSettingsForStoargeAccount(accountId);
            var model = new WebDavConfigurationModel
            {
                ServerUrl = settings.Where(s => string.Equals("server", s.SettingName, StringComparison.OrdinalIgnoreCase))
                            .Select(s => s.SettingValue)
                            .FirstOrDefault(),
                AccountLogin = settings.Where(s => string.Equals("login", s.SettingName, StringComparison.OrdinalIgnoreCase))
                            .Select(s => s.SettingValue)
                            .FirstOrDefault(),
            };

            string accountPasswordEncrypted = settings.Where(s => string.Equals("password", s.SettingName, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.SettingValue)
                                .FirstOrDefault();
            if (!string.IsNullOrEmpty(accountPasswordEncrypted))
                model.AccountPassword = _cryptoService.DecryptString(accountPasswordEncrypted, _securityConfiguration.EncryptionKey, _securityConfiguration.EncryptionSalt);

            return model;
        }

        public object GetAccountConfigurationModel()
        {
            return new WebDavConfigurationModel();
        }

        public void ApplyConfiguration(int accountId, DateTime accountStamp, object configurationModel)
        {
            WebDavConfigurationModel webDavConfigurationModel = configurationModel as WebDavConfigurationModel;
            if (webDavConfigurationModel == null)
                return;

            Dictionary<string, string> settings = new Dictionary<string, string>
                {
                    {"login", webDavConfigurationModel.AccountLogin}, 
                    {"password", _cryptoService.EncryptString(webDavConfigurationModel.AccountPassword, _securityConfiguration.EncryptionKey, _securityConfiguration.EncryptionSalt)}, 
                    {"server", webDavConfigurationModel.ServerUrl}
                };
            _accountService.SaveSettings(settings, accountId, accountStamp);
        }
    }
}
