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
using System.IO;

namespace StorageMonster.Plugin.WebDav
{
    public class WebDavPlugin : IStoragePlugin
    {
        public virtual string Name { get { return "WebDav"; } }

        protected IStorageAccountService AccountService { get; set; }
        protected ICryptoService CryptoService { get; set; }
        //protected ISecurityConfiguration SecurityConfiguration { get; set; }

        //protected WeakRefHolder<ConfigurationProvider> ConfigurationProviderReference;
        protected WeakRefHolder<FolderQuery> FolderQueryReference;
        protected WeakRefHolder<FileStreamQuery> FileStreamQueryReference;

        //public WebDavPlugin(IStorageAccountService accountService, 
        //    ICryptoService cryptoService,
        //    ISecurityConfiguration securityConfiguration)
        //{
        //    AccountService = accountService;
        //    CryptoService = cryptoService;
        //    SecurityConfiguration = securityConfiguration;
        //    ConfigurationProviderReference = new WeakRefHolder<ConfigurationProvider>(() => new ConfigurationProvider(AccountService, CryptoService, SecurityConfiguration));
        //    FolderQueryReference = new WeakRefHolder<FolderQuery>(() => new FolderQuery());
        //    FileStreamQueryReference = new WeakRefHolder<FileStreamQuery>(() => new FileStreamQuery());
        //}

        //public virtual object GetAccountConfigurationModel(int accountId)
        //{
        //    return ConfigurationProviderReference.Target.GetAccountConfigurationModel(accountId);
        //}

        //public virtual object GetAccountConfigurationModel()
        //{
        //    return ConfigurationProviderReference.Target.GetAccountConfigurationModel();
        //}

        //public virtual void ApplyConfiguration(int accountId, DateTime accountStamp, object configurationModel)
        //{
        //    ConfigurationProviderReference.Target.ApplyConfiguration(accountId, accountStamp, configurationModel);            
        //}

        //protected static PluginException TranslateSharpboxException(SharpBoxException exception)
        //{
        //    if (exception == null)
        //        throw new ArgumentNullException("exception");

        //    PluginErrorCodes errorCode;
        //    switch (exception.ErrorCode)
        //    {
        //        case SharpBoxErrorCodes.ErrorCouldNotContactStorageService:
        //            errorCode = PluginErrorCodes.CouldNotContactStorageService;
        //            break;
        //        case SharpBoxErrorCodes.ErrorCouldNotRetrieveDirectoryList:
        //            errorCode = PluginErrorCodes.CouldNotRetrieveDirectoryList;
        //            break;
        //        case SharpBoxErrorCodes.ErrorCreateOperationFailed:
        //            errorCode = PluginErrorCodes.CreateOperationFailed;
        //            break;
        //        case SharpBoxErrorCodes.ErrorFileNotFound:
        //            errorCode = PluginErrorCodes.FileNotFound;
        //            break;
        //        case SharpBoxErrorCodes.ErrorInsufficientDiskSpace:
        //            errorCode = PluginErrorCodes.InsufficientDiskSpace;
        //            break;
        //        case SharpBoxErrorCodes.ErrorInvalidCredentialsOrConfiguration:
        //            errorCode = PluginErrorCodes.InvalidCredentialsOrConfiguration;
        //            break;
        //        case SharpBoxErrorCodes.ErrorInvalidFileOrDirectoryName:
        //            errorCode = PluginErrorCodes.InvalidFileOrDirectoryName;
        //            break;
        //        case SharpBoxErrorCodes.ErrorTransferAbortedManually:
        //            errorCode = PluginErrorCodes.TransferAbortedManually;
        //            break;
        //        default:
        //            errorCode = PluginErrorCodes.PluginError;
        //            break;
        //    }

        //    return new PluginException(errorCode, "Error", exception);
        //}

        //protected static IStorageQueryExecutor BuildQueryExecutor(Func<StorageQueryResult> action)
        //{
        //    return StorageQueryExecutor.WithAction(action)
        //        .IfExceptionIs(typeof(UriFormatException))
        //            .Throw((ex) => new PluginException(PluginErrorCodes.InvalidCredentialsOrConfiguration, "Invalid url", ex))
        //        .IfExceptionIs(typeof(UnauthorizedAccessException))
        //            .Throw((ex) => new PluginException(PluginErrorCodes.InvalidCredentialsOrConfiguration, "Unauthorized", ex))
        //        .IfExceptionIs(typeof(SharpBoxException))
        //            .Throw((ex) => TranslateSharpboxException((SharpBoxException)ex));
        //}

        //public virtual StorageFileStreamResult GetFileStream(string fileUrl, int accountId)
        //{
        //    WebDavConfigurationModel model = GetAccountConfigurationModel(accountId) as WebDavConfigurationModel;
        //    return BuildQueryExecutor(() => FileStreamQueryReference.Target.Execute(model, fileUrl)).Run<StorageFileStreamResult>();
        //}
               

        //public virtual StorageFolderResult QueryStorage(int accountId, string path)
        //{
        //    WebDavConfigurationModel model = GetAccountConfigurationModel(accountId) as WebDavConfigurationModel;
        //    return BuildQueryExecutor(() => FolderQueryReference.Target.Execute(model, path, accountId)).Run<StorageFolderResult>();            
        //}		


        public object GetAccountConfigurationModel(int accountId)
        {
            throw new NotImplementedException();
        }

        public object GetAccountConfigurationModel()
        {
            throw new NotImplementedException();
        }

        public void ApplyConfiguration(int accountId, DateTime accountStamp, object configurationModel)
        {
            throw new NotImplementedException();
        }

        public StorageFolderResult QueryStorage(int accountId, string path)
        {
            throw new NotImplementedException();
        }

        public StorageFileStreamResult GetFileStream(string fileUrl, int accountId)
        {
            throw new NotImplementedException();
        }
    }
}