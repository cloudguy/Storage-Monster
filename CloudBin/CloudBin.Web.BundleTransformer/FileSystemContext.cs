using BundleTransformer.Core.FileSystem;

namespace CloudBin.Web.BundleTransformer
{
    internal sealed class FileSystemContext : IFileSystemContext
    {
        private readonly IVirtualFileSystemWrapper _virtualFilesSystemWrapper;
        private readonly IRelativePathResolver _relativePathResolver;

        internal FileSystemContext(IVirtualFileSystemWrapper virtualFilesSystemWrapper, IRelativePathResolver relativePathResolver)
        {
            _virtualFilesSystemWrapper = virtualFilesSystemWrapper;
            _relativePathResolver = relativePathResolver;
        }

        IVirtualFileSystemWrapper IFileSystemContext.GetVirtualFileSystemWrapper()
        {
            return _virtualFilesSystemWrapper;
        }

        IRelativePathResolver IFileSystemContext.GetCommonRelativePathResolver()
        {
            return _relativePathResolver;
        }
    }
}

