using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudBin.Web.Core
{
    //thanks to http://hugoware.net/blog/ignoring-case-with-mono-mvc
    public sealed class CaseInsensitiveViewEngine : RazorViewEngine
    {
        private static readonly string Root = HttpContext.Current.Server.MapPath("~/");

        //adds a new CaseInsensitiveViewEngine to the routes provided
        public static void Register(ViewEngineCollection engines)
        {
            //clear the existing WebForm View Engines
            IViewEngine[] razors = engines.Where(engine => engine is RazorViewEngine).ToArray();
            foreach (IViewEngine engine in razors)
            {
                ViewEngines.Engines.Remove(engine);
            }

            //add the new case-insensitive engine
            ViewEngines.Engines.Add(new CaseInsensitiveViewEngine());
        }

        //holds all of the actual paths to the required files
        private static readonly ConcurrentDictionary<string, string> ViewPaths = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        //update the path to match a real file
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            viewPath = GetActualFilePath(viewPath);
            masterPath = GetActualFilePath(masterPath);
            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        //finds partial views by detecting matches
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            partialPath = GetActualFilePath(partialPath);
            return base.CreatePartialView(controllerContext, partialPath);
        }

        //perform a case-insensitive file search
        protected override bool FileExists(ControllerContext context, string virtualPath)
        {
            virtualPath = GetActualFilePath(virtualPath);
            return base.FileExists(context, virtualPath);
        }

        //determines (and caches) the actual path for a file
        private string GetActualFilePath(string virtualPath)
        {
            //check if this has already been matched before
            if (ViewPaths.ContainsKey(virtualPath))
                return ViewPaths[virtualPath];

            //break apart the path
            string[] segments = virtualPath.Split(new[] {'/'});

            //get the root folder to work from
            DirectoryInfo folder = new DirectoryInfo(Root);

            //start stepping up the folders to replace with the correct cased folder name
            for (int i = 0; i < segments.Length; i++)
            {
                string part = segments[i];
                bool last = i == segments.Length - 1;

                //ignore the root
                if (part.Equals("~")) continue;

                //process the file name if this is the last segment
                part = last ? GetFileName(part, folder) : GetDirectoryName(part, ref folder);

                //if no matches were found, just return the original string
                if (part == null || folder == null) return virtualPath;

                //update the segment with the correct name
                segments[i] = part;
            }

            //save this path for later use
            virtualPath = string.Join("/", segments);
            //CaseInsensitiveViewEngine._ViewPaths.Remove(virtualPath);
            ViewPaths[virtualPath] = virtualPath;
            return virtualPath;
        }

        //searches for a matching file name in the current directory
        private string GetFileName(string part, DirectoryInfo folder)
        {
            //try and find a matching file, regardless of case
            FileInfo match = folder.GetFiles().FirstOrDefault(file => file.Name.Equals(part, StringComparison.OrdinalIgnoreCase));
            return match != null ? match.Name : null;
        }

        //searches for a folder in the current directory and steps up a level
        private string GetDirectoryName(string part, ref DirectoryInfo folder)
        {
            folder = folder.GetDirectories().FirstOrDefault(dir => dir.Name.Equals(part, StringComparison.OrdinalIgnoreCase));
            return folder != null ? folder.Name : null;
        }
    }
}
