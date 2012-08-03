using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Concurrent;

namespace StorageMonster.Web.Services
{
    //http://hugoware.net/blog/ignoring-case-with-mono-mvc
    public class CaseInsensitiveViewEngine : WebFormViewEngine
    {
        private static string _Root = HttpContext.Current.Server.MapPath("~/");

        //adds a new CaseInsensitiveViewEngine to the routes provided
        public static void Register(ViewEngineCollection engines)
        {

            //clear the existing WebForm View Engines
            IViewEngine[] webforms = engines.Where(engine =>
              engine is WebFormViewEngine).ToArray();
            foreach (IViewEngine engine in webforms)
                ViewEngines.Engines.Remove(engine);

            //add the new case-insensitive engine
            ViewEngines.Engines.Add(new CaseInsensitiveViewEngine());

        }

        //holds all of the actual paths to the required files
        private static ConcurrentDictionary<string, string> _ViewPaths =
            new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        //update the path to match a real file
        protected override IView CreateView(ControllerContext controllerContext,
        string viewPath, string masterPath)
        {
            viewPath = this._GetActualFilePath(viewPath);
            masterPath = this._GetActualFilePath(masterPath);
            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        //finds partial views by detecting matches
        protected override IView CreatePartialView(ControllerContext controllerContext,
          string partialPath)
        {
            partialPath = this._GetActualFilePath(partialPath);
            return base.CreatePartialView(controllerContext, partialPath);
        }

        //perform a case-insensitive file search
        protected override bool FileExists(ControllerContext context, string virtualPath)
        {
            virtualPath = this._GetActualFilePath(virtualPath);
            return base.FileExists(context, virtualPath);
        }

        //determines (and caches) the actual path for a file
        private string _GetActualFilePath(string virtualPath)
        {

            //check if this has already been matched before
            if (CaseInsensitiveViewEngine._ViewPaths.ContainsKey(virtualPath))
                return CaseInsensitiveViewEngine._ViewPaths[virtualPath];

            //break apart the path
            string[] segments = virtualPath.Split(new char[] { '/' });

            //get the root folder to work from
            var folder = new DirectoryInfo(CaseInsensitiveViewEngine._Root);

            //start stepping up the folders to replace with the correct cased folder name
            for (int i = 0; i < segments.Length; i++)
            {
                string part = segments[i];
                bool last = i == segments.Length - 1;

                //ignore the root
                if (part.Equals("~")) continue;

                //process the file name if this is the last segment
                else if (last) part = this._GetFileName(part, folder);

                //step up the directory for another part
                else part = this._GetDirectoryName(part, ref folder);

                //if no matches were found, just return the original string
                if (part == null || folder == null) return virtualPath;

                //update the segment with the correct name
                segments[i] = part;

            }

            //save this path for later use
            virtualPath = string.Join("/", segments);
            //CaseInsensitiveViewEngine._ViewPaths.Remove(virtualPath);
            CaseInsensitiveViewEngine._ViewPaths[virtualPath] = virtualPath;
            return virtualPath;
        }

        //searches for a matching file name in the current directory
        private string _GetFileName(string part, DirectoryInfo folder)
        {

            //try and find a matching file, regardless of case
            FileInfo match = folder.GetFiles().FirstOrDefault(file =>
                file.Name.Equals(part, StringComparison.OrdinalIgnoreCase));
            return match is FileInfo ? match.Name : null;
        }

        //searches for a folder in the current directory and steps up a level
        private string _GetDirectoryName(string part, ref DirectoryInfo folder)
        {
            folder = folder.GetDirectories().FirstOrDefault(dir =>
                dir.Name.Equals(part, StringComparison.OrdinalIgnoreCase));
            return folder is DirectoryInfo ? folder.Name : null;
        }

    }
}