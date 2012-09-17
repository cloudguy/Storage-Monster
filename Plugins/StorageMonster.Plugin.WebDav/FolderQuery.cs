using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppLimit.CloudComputing.SharpBox.StorageProvider.WebDav;
using AppLimit.CloudComputing.SharpBox;
using AppLimit.CloudComputing.SharpBox.StorageProvider;
using StorageMonster.Utilities;
using System.Globalization;
using StorageMonster.Plugin.StorageQuery;

namespace StorageMonster.Plugin.WebDav
{
    public class FolderQuery
    {
        public StorageFolderResult Execute(WebDavConfigurationModel model, string path, int accountId)
        {
            model = model.With(m => !string.IsNullOrEmpty(m.ServerUrl))
                   .With(m => !string.IsNullOrEmpty(m.AccountLogin))
                   .With(m => !string.IsNullOrEmpty(m.AccountPassword));

            if (model == null)
                throw new PluginException(PluginErrorCodes.InvalidCredentialsOrConfiguration);

            StorageFolderResult result = new StorageFolderResult();

            Uri uri = new Uri(model.ServerUrl);
            ICloudStorageConfiguration config = new WebDavConfiguration(uri);
            GenericNetworkCredentials cred = new GenericNetworkCredentials();
            cred.UserName = model.AccountLogin;
            cred.Password = model.AccountPassword;

            if (string.IsNullOrEmpty(path))
                path = "/";
            else
                path = path.RemoveCharDuplicates('/');
            if (!path.Equals("/", StringComparison.OrdinalIgnoreCase))
                path = path.TrimEnd('/');

            CloudStorage storage = null;
            try
            {
                storage = new CloudStorage();
                ICloudStorageAccessToken storageToken = storage.Open(config, cred);
                ICloudDirectoryEntry directory = storage.GetFolder(path, true);

                result.CurrentFolderName = directory.Name;
                result.CurrentFolderUrl = path;
                path = path.TrimEnd('/');
                foreach (var entry in directory)
                {
                    var dirEntry = entry as ICloudDirectoryEntry;
                    string entryPath = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", path, "/", entry.Name);
                    if (dirEntry != null)
                    {
                        result.AddItem(new StorageFolder
                        {
                            Name = dirEntry.Name,
                            Path = entryPath,
                            StorageAccountId = accountId
                        });
                    }
                    else
                    {
                        result.AddItem(new StorageFile
                        {
                            Name = entry.Name,
                            Path = entryPath,
                            StorageAccountId = accountId
                        });
                    }
                }

                StoragePathItem rootPath = new StoragePathItem
                {
                    Name = "/",
                    Url = "/"
                };
                string relativePath = path.Trim('/').RemoveCharDuplicates('/');
                if (relativePath.Length > 0)
                {
                    string[] pathItems = relativePath.Split('/');
                    StringBuilder pathUrlsBuilder = new StringBuilder("/");
                    foreach (var pathItem in pathItems)
                    {
                        pathUrlsBuilder.Append(pathItem);
                        rootPath.AppendItem(new StoragePathItem
                            {
                                Name = pathItem,
                                Url = pathUrlsBuilder.ToString()
                            });
                        pathUrlsBuilder.Append("/");
                    }
                }

                result.CurrentPath = rootPath;
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
