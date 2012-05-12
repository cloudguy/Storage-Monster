using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppLimit.CloudComputing.SharpBox;
using AppLimit.CloudComputing.SharpBox.StorageProvider;
using AppLimit.CloudComputing.SharpBox.StorageProvider.WebDav;

namespace StorageMonster.Plugin.WebDav
{
    public class WebDavPlugin : IStoragePlugin
    {
        public virtual string Name { get { return "WebDav"; } }

		public void Test()
		{
			Uri u = new Uri("https://webdav.yandex.ru");
			ICloudStorageConfiguration config = new WebDavConfiguration(u);

            
			
			GenericNetworkCredentials cred = new GenericNetworkCredentials();
			cred.UserName = "cloudguy";
			cred.Password = "---";

			CloudStorage storage = new CloudStorage();
			ICloudStorageAccessToken storageToken = storage.Open(config, cred);


		  //  storage.GetCloudConfiguration(nSupportedCloudConfigurations.WebDav);
// After successful login you may do the necessary Directory/File manipulations by the SharpBox API
// Here is the most often and simplest one
			ICloudDirectoryEntry root = storage.GetRoot();

		    var f =storage.GetFolder("/");
            //f.First().

            var c =storage.GetCloudConfiguration(nSupportedCloudConfigurations.WebDav);
			//storage.
			storage.Close();
		}
    }
}