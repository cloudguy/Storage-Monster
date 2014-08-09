namespace CloudBin.Web.Utilities.Configuration
{
    public interface IWebConfiguration
    {
        bool DoNotOpenDbSessionForScriptAndContent { get; }
        bool CompressDynamicContent { get; }
        bool CompressStaticContent { get; }
        bool SendSecurityHeaders { get; }
        bool RemoveVersionHeaders { get; }
    }
}
