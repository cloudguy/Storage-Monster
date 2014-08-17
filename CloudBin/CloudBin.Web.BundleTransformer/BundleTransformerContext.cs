using BundleTransformer.Core;
using BundleTransformer.Core.Assets;
using BundleTransformer.Core.FileSystem;

namespace CloudBin.Web.BundleTransformer
{
    internal sealed class BundleTransformerContext : IBundleTransformerContext
    {
        private readonly IBundleTransformerContext _innerContext;
        private readonly IFileSystemContext _fileSystemContext;

        internal BundleTransformerContext(IBundleTransformerContext innerContext)
        {
            _innerContext = innerContext;
            _fileSystemContext = new FileSystemContext(new VirtualFileSystemWrapper(_innerContext.FileSystem.GetVirtualFileSystemWrapper()), _innerContext.FileSystem.GetCommonRelativePathResolver());
        }

        global::BundleTransformer.Core.Configuration.IConfigurationContext IBundleTransformerContext.Configuration
        {
            get { return _innerContext.Configuration; }
        }

        IFileSystemContext IBundleTransformerContext.FileSystem
        {
            get { return _fileSystemContext; }
        }

        IAssetContext IBundleTransformerContext.Styles
        {
            get { return _innerContext.Styles; }
        }

        IAssetContext IBundleTransformerContext.Scripts
        {
            get { return _innerContext.Scripts; }
        }

        bool IBundleTransformerContext.IsDebugMode
        {
            get { return _innerContext.IsDebugMode; }
        }
    }
}
