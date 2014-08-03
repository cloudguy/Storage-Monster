namespace CloudBin.Web.Utilities
{
    public static class Constants
    {
        public const string AjaxHeaderName = "X-Requested-With";
        public const string AjaxHeaderValue = "XMLHttpRequest";
        public const string AcceptEncodingHeaderName = "Accept-Encoding";
        public const string ContentEncodingHeaderName = "Content-Encoding";
        public const string VaryHeaderName = "Vary";
        public const string XssProtectionHeaderName = "X-XSS-Protection";
        public const string FrameOptionsHeaderName = "X-Frame-Options";
        public const string ContentTypeOptionsHeaderName = "X-Content-Type-Options";

        public const string AxdRequestPattern = @"^/*[\w%]+\.axd.*";

        public const string ScriptsFolderName = "Scripts";
        public const string ContentFolderName = "Content";

        public const string GzipEncodingHeaderValue = "gzip";
        public const string DeflateEncodingHeaderValue = "deflate";
        public const string XssProtectionEnabledHeaderValue = "1; mode=block";
        public const string FrameOptionsDenyHeaderValue = "deny";
        public const string ContentTypeOptionsNosniffHeaderValue = "nosniff";
    }
}
