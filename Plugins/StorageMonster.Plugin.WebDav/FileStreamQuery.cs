using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Utilities;
using AppLimit.CloudComputing.SharpBox;
using AppLimit.CloudComputing.SharpBox.StorageProvider.WebDav;
using AppLimit.CloudComputing.SharpBox.StorageProvider;

namespace StorageMonster.Plugin.WebDav
{
    public class FileStreamQuery
    {
        public virtual StorageFileStreamResult Execute(WebDavConfigurationModel model, string filePath)
        {
            model = model.With(m => !string.IsNullOrEmpty(m.ServerUrl))
                    .With(m => !string.IsNullOrEmpty(m.AccountLogin))
                    .With(m => !string.IsNullOrEmpty(m.AccountPassword));

            if (model == null)
                throw new PluginException(PluginErrorCodes.InvalidCredentialsOrConfiguration);

            if (string.IsNullOrEmpty(filePath))
                throw new PluginException(PluginErrorCodes.InvalidFileOrDirectoryName);
            else
                filePath = filePath.TrimEnd('/').RemoveCharDuplicates('/');

            if (string.IsNullOrEmpty(filePath))
                throw new PluginException(PluginErrorCodes.InvalidFileOrDirectoryName);

            StorageFileStreamResult result = new StorageFileStreamResult();

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
                var file = storage.GetFile(filePath, null);
                result.FileName = file.Name;
                result.FileStream = file.GetDataTransferAccessor().GetDownloadStream();
            }
            finally
            {
                if (storage != null)
                    storage.Close();
            }

            return result;
        }
    }
}
