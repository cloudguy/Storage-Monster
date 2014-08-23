namespace CloudBin.Web.Core.Configuration
{
    public interface IWebConfiguration
    {
        bool DoNotOpenDbSessionForScriptAndContent { get; }
        bool CompressDynamicContent { get; }
        bool CompressBundledContent { get; }
        bool CompressStaticContent { get; }
        bool SendSecurityHeaders { get; }
        bool RemoveVersionHeaders { get; }
        bool MinifyHtml { get; }
        string TrackingCookieName { get; }
    }
}
