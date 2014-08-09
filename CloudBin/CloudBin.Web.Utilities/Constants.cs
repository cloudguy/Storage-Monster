namespace CloudBin.Web.Utilities
{
    public static class Constants
    {
        public static readonly string AjaxHeaderName = "X-Requested-With";
        public static readonly string AjaxHeaderValue = "XMLHttpRequest";
        public static readonly string AcceptEncodingHeaderName = "Accept-Encoding";
        public static readonly string ContentEncodingHeaderName = "Content-Encoding";
        public static readonly string VaryHeaderName = "Vary";
        public static readonly string XssProtectionHeaderName = "X-XSS-Protection";
        public static readonly string FrameOptionsHeaderName = "X-Frame-Options";
        public static readonly string ContentTypeOptionsHeaderName = "X-Content-Type-Options";

        public static readonly string AxdRequestPattern = @"^/*[\w%]+\.axd.*";

        public static readonly string ScriptsFolderName = "Scripts";
        public static readonly string ContentFolderName = "Content";
        public static readonly string BundlesRootPath = "bundles";

        public static readonly string GzipEncodingHeaderValue = "gzip";
        public static readonly string DeflateEncodingHeaderValue = "deflate";
        public static readonly string XssProtectionEnabledHeaderValue = "1; mode=block";
        public static readonly string FrameOptionsDenyHeaderValue = "deny";
        public static readonly string ContentTypeOptionsNosniffHeaderValue = "nosniff";

        public static readonly string PoweredByHeaderName = "X-Powered-By";
        public static readonly string AspNetVersionHeaderName = "X-AspNet-Version";
        public static readonly string AspNetMvcVersionHeaderName = "X-AspNetMvc-Version";
        public static readonly string ServerHeaderName = "Server";
    }
}
