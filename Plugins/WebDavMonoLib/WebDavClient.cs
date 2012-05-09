using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace WebDavMonoLib
{
    public class WebDavClient
    {
        public string ServerUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }

        // Server url should be full url, including protocol, like http://localhost
        public WebDavClient(string serverUrl, string userName, string password, string domain)
        {
            ServerUrl = serverUrl;
            UserName = userName;
            Password = password;
            Domain = domain;
        }

        public List<string> GetListItems(string folderUrl)
        {
            string rawContents = GetRawListItems(folderUrl);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(rawContents);

            // to get list of all <a:response> elements
            var nodeList = xmlDoc.ChildNodes[1].ChildNodes;

            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("a", "DAV:");
            XmlNode tempNode;

            var items = new List<string>();

            foreach (XmlNode node in nodeList)
            {
                tempNode = node.FirstChild;

                // uncomment to get 'Display name' of item
                // tempNode = node.SelectSingleNode("a:propstat/a:prop/a:displayname", nsmgr);
                items.Add(tempNode.InnerText);
            }
            return items;
        }

        public void Copy(string copyWhat, string copyTo)
        {
            string fullCopyToUrl = ServerUrl.TrimEnd("/".ToCharArray()) + copyTo.TrimEnd("/".ToCharArray()) + "/" + Path.GetFileName(copyWhat);
            IDictionary<string, string> headers = new Dictionary<string, string> { { "Destination", fullCopyToUrl } };
            SendRequest(copyWhat, "COPY", null, null, headers);
        }

        public void Move(string moveWhat, string moveTo)
        {
            string fullMoveTo = ServerUrl.TrimEnd("/".ToCharArray()) + moveTo.TrimEnd("/".ToCharArray()) + "/" + Path.GetFileName(moveWhat);
            IDictionary<string, string> headers = new Dictionary<string, string> { { "Destination", fullMoveTo } };
            SendRequest(moveWhat, "MOVE", null, null, headers);
        }

        public string ReadItemProperties(string itemPath)
        {
            string result = SendRequest(itemPath, "PROPFIND", null, "0", null);
            return result;
        }

        public void UploadItem(string folderUrl, string localFilePath, string targetFileName = null)
        {
            var fileName = Path.GetFileName(localFilePath);
            var webClient = GetWebClient();
            string path;

            if (targetFileName == null)
            {
                path = ServerUrl + folderUrl.TrimEnd("/".ToCharArray()) + "/" + Path.GetFileName(localFilePath).Trim("/".ToCharArray());
            }
            else
            {
                path = ServerUrl + folderUrl.TrimEnd("/".ToCharArray()) + "/" + targetFileName;
            }

            webClient.UploadFile(path, "PUT", localFilePath);
        }


        private WebClient GetWebClient()
        {
            var webClient = new WebClient();
            var cache = new CredentialCache
                {
                    {
                        new Uri(ServerUrl), "Negotiate",
                        new NetworkCredential(UserName, Password, Domain)
                        }
                };
            webClient.Credentials = cache;
            return webClient;
        }

        public string DownloadItem(string remotePath, string localPath)
        {
            //  SendRequest(remotePath, "GET", fullLocalPath, credentialName, null, null);         
            var webClient = GetWebClient();
            webClient.DownloadFile(ServerUrl + remotePath, localPath);
            return localPath;
        }

        public string CreateWebDavFolder(string newFolderPath)
        {
            SendRequest(newFolderPath, "MKCOL", null, null, null);
            return newFolderPath;
        }

        public string CreateWebDavFolder(string folderPath, string folderName)
        {
            var fullPath = folderPath.TrimEnd("/".ToCharArray()) + @"/" + folderName;
            SendRequest(fullPath, "MKCOL", null, null, null);
            return fullPath;
        }

        public void Delete(string itemPath)
        {
            SendRequest(itemPath, "DELETE", null, null, null);
        }

        public string GetRawListItems(string folder)
        {
            string result = SendRequest(folder, "PROPFIND", null, "1", null);
            return result;
        }

        private string SendRequest(string url, string method, string localFilePath, string depth, IDictionary<string, string> headers)
        {
            string fullUrl = ServerUrl;
            fullUrl += url;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(fullUrl);

            Uri uri = new Uri(fullUrl);

            // Need to send along headers?
            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    httpWebRequest.Headers.Set(key, headers[key]);
                }
            }

            NetworkCredential networkCredential = new NetworkCredential(UserName, Password);
            CredentialCache credentialCache = new CredentialCache { { uri, "Basic", networkCredential } };
            httpWebRequest.Credentials = credentialCache;

            httpWebRequest.Credentials = networkCredential;

            // Send authentication along with first request.
            httpWebRequest.PreAuthenticate = true;

            if (depth != null)
            {
                httpWebRequest.Headers.Add("Depth", depth);
            }

            httpWebRequest.Method = method;

            httpWebRequest.ContentType = "image/jpeg";
            httpWebRequest.UserAgent = "Microsoft-WebDAV-MiniRedir/6.1.7600";

            httpWebRequest.Headers.Add("translate", "f");

            httpWebRequest.AllowWriteStreamBuffering = true;

            Stream sr = httpWebRequest.GetRequestStream();
#warning
            //string allprop =  "<?xml version=\"1.0\" encoding=\"utf-8\" ?><D:propfind xmlns:D=\"DAV:\"><D:allprop/></D:propfind>";

#warning encoding
            sr.Write(Encoding.ASCII.GetBytes(string.Empty), 0, 0);
            sr.Close();

            // execute the request
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();

            // we will read data via the response stream
            Stream resStream = response.GetResponseStream();

            StringBuilder sb = new StringBuilder();
            int totalread = 0;

            if (method == "GET")
            {
                // read into buffer and write to disk until whole chunk read
                BinaryReader br = new BinaryReader(resStream);

                FileStream fs = new FileStream(localFilePath, FileMode.Create, FileAccess.Write);

                const int buffsize = 1024;
                byte[] bytes = new byte[buffsize];

                int numread = buffsize;
                while (numread != 0)
                {
                    // read from source
                    numread = br.Read(bytes, 0, buffsize);
                    totalread += numread;

                    // write to disk
                    fs.Write(bytes, 0, numread);
                }

                br.Close();
                fs.Close();
            }

            else
            {
                int count;
                byte[] buf = new byte[8192];

                do
                {
                    // fill the buffer with data
                    count = resStream.Read(buf, 0, buf.Length);

                    // make sure we read some data
                    if (count != 0)
                    {
                        // translate from bytes to ASCII text
                        string tempString = Encoding.ASCII.GetString(buf, 0, count);

                        // continue building the string
                        sb.Append(tempString);
                    }
                }
                while (count > 0); // any more data to read?

                // print out page source
            }

            response.Close();
            resStream.Close();
            return sb.ToString();
        }
    }
}


#warning remove this shit
/*
 WebDavClient client = new WebDavClient("https://webdav.yandex.ru", "cloudguy", "---", "https://webdav.yandex.ru");

var r = client.GetListItems(@"/");
  client.CreateWebDavFolder(@"/webdav/Test/Building%20Blocks/test1", "test2");
client.CreateWebDavFolder(@"/webdav/Test/Building%20Blocks/test1");
client.Copy(@"/webdav/Test/Building%20Blocks/Default%20Multimedia%20Schema.xsd", @"/webdav/Test/Building%20Blocks/test");
client.Delete(@"/webdav/Test/Building%20Blocks/test/Default%20Multimedia%20Schema.xsd");
client.DownloadItem(@"/webdav/Test/Building%20Blocks/Default%20Multimedia%20Schema.xsd", @"c:\schema.xsd");
client.GetListItems(@"/webdav/Test/Building%20Blocks");
client.GetRawListItems(@"/webdav/Test/Building%20Blocks");
client.Move(@"/webdav/Test/Building%20Blocks/test1/Default%20Multimedia%20Schema.xsd", @"/webdav/Test/Building%20Blocks/test");
client.ReadItemProperties(@"/webdav/Test/Building%20Blocks/test/Default%20Multimedia%20Schema.xsd");
client.UploadItem(@"/webdav/Test/Building%20Blocks/test", @"C:\SDLwallpaper.jpg");
client.UploadItem(@"/webdav/Test/Building%20Blocks/test", @"C:\SDLwallpaper.jpg", "New MM Component.jpg"); */