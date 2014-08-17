using BundleTransformer.Core.FileSystem;
using System;
using System.IO;
using System.Text;
using System.Web.Caching;

namespace CloudBin.Web.BundleTransformer
{
    //mono fix for IVirtualFileSystemWrapper.ToAbsolutePath (string virtualPath)
    internal sealed class VirtualFileSystemWrapper : IVirtualFileSystemWrapper
    {
        private readonly IVirtualFileSystemWrapper _innerWrapper;

        internal VirtualFileSystemWrapper(IVirtualFileSystemWrapper innerWrapper)
        {
            _innerWrapper = innerWrapper;
        }

        bool IVirtualFileSystemWrapper.FileExists(string virtualPath)
        {
            return _innerWrapper.FileExists(virtualPath);
        }

        string IVirtualFileSystemWrapper.GetFileTextContent(string virtualPath)
        {
            return _innerWrapper.GetFileTextContent(virtualPath);
        }

        byte[] IVirtualFileSystemWrapper.GetFileBinaryContent(string virtualPath)
        {
            return _innerWrapper.GetFileBinaryContent(virtualPath);
        }

        Stream IVirtualFileSystemWrapper.GetFileStream(string virtualPath)
        {
            return _innerWrapper.GetFileStream(virtualPath);
        }

        string IVirtualFileSystemWrapper.ToAbsolutePath(string virtualPath)
        {
            return System.Web.VirtualPathUtility.ToAbsolute(virtualPath);
        }

        string IVirtualFileSystemWrapper.GetCacheKey(string virtualPath)
        {
            return _innerWrapper.GetCacheKey(virtualPath);
        }

        CacheDependency IVirtualFileSystemWrapper.GetCacheDependency(string virtualPath, string[] virtualPathDependencies, DateTime utcStart)
        {
            return _innerWrapper.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        bool IVirtualFileSystemWrapper.IsTextFile(string virtualPath, int sampleSize, out Encoding encoding)
        {
            return _innerWrapper.IsTextFile(virtualPath, sampleSize, out encoding);
        }
    }
}
