namespace CloudBin.Web.Utilities
{
    public interface IWebConfiguration
    {
        bool DoNotOpenDbSessionForScriptAndContent { get; }
        bool CompressDynamicContent { get; }
        bool CompressStaticContent { get; }
        bool SendSecurityHeaders { get; }
    }
}
