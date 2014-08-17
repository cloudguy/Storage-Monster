using System.Web;

namespace CloudBin.Web.Core.Bundling
{
    public interface IBundleProvider
    {
        void RegisterScriptBundle(string bundleName, params string[] scriptVirtualPaths);
        void RegisterStyleBundle(string bundleName, params string[] styleVirtualPaths);
        IHtmlString GetBundleUrl(string bundleName);
        bool IsBundleRequest();
        bool EnableOptimizations { get; set; }
        void Initialize();
    }
}
