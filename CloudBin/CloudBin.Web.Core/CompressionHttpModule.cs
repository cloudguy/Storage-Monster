using System;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CloudBin.Web.Core.Bundling;
using CloudBin.Web.Core.Configuration;

namespace CloudBin.Web.Core
{
    //use iis compression if available
    public sealed class CompressionHttpModule : IHttpModule
    {
        private static readonly Lazy<IWebConfiguration> LazyWebConfiguration = new Lazy<IWebConfiguration>(() =>
        {
            return (IWebConfiguration) System.Web.Mvc.DependencyResolver.Current.GetService(typeof (IWebConfiguration));
        }, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private static readonly Lazy<IBundleProvider> LazyBundleProvider = new Lazy<IBundleProvider>(() =>
        {
            return (IBundleProvider)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IBundleProvider));
        }, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private IWebConfiguration WebConfiguration
        {
            get { return LazyWebConfiguration.Value; }
        }

        private enum CompressionType
        {
            NoCompression = 0,
            Gzip = 1,
            Deflate = 2
        }

        void IHttpModule.Dispose()
        {
        }

        void IHttpModule.Init(HttpApplication application)
        {
            application.PostRequestHandlerExecute += application_PostRequestHandlerExecute;
        }

        private void application_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication) sender;
            HttpResponse response = application.Response;

            if (response.Filter == null)
            {
                return;
            }

            if (LazyBundleProvider.Value.IsBundleRequest() && !WebConfiguration.CompressBundledContent)
            {
                return;
            }

            IHttpHandler handler = application.Context.CurrentHandler;
            bool isDynamicContent = handler is Page || handler is MvcHandler;
            if (isDynamicContent && !WebConfiguration.CompressDynamicContent)
            {
                return;
            }

            if (!isDynamicContent && !WebConfiguration.CompressStaticContent)
            {
                return;
            }

            CompressionType compressionType = GetSupportedCompression(application.Request);

            switch (compressionType)
            {
                case CompressionType.Gzip:
                    response.Filter = new System.IO.Compression.GZipStream(response.Filter, System.IO.Compression.CompressionMode.Compress);
                    response.AppendHeader(Constants.ContentEncodingHeaderName, Constants.GzipEncodingHeaderValue);
                    break;
                case CompressionType.Deflate:
                    response.Filter = new System.IO.Compression.DeflateStream(response.Filter, System.IO.Compression.CompressionMode.Compress);
                    response.AppendHeader(Constants.ContentEncodingHeaderName, Constants.DeflateEncodingHeaderValue);
                    break;
            }

            // Allow proxy servers to cache encoded and unencoded versions separately
            response.AppendHeader(Constants.VaryHeaderName, Constants.ContentEncodingHeaderName);
        }

        private static CompressionType GetSupportedCompression(HttpRequest request)
        {
            string acceptEncodingHeader = request.Headers[Constants.AcceptEncodingHeaderName];

            if (string.IsNullOrWhiteSpace(acceptEncodingHeader))
            {
                return CompressionType.NoCompression;
            }

            if (acceptEncodingHeader.IndexOf(Constants.GzipEncodingHeaderValue, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return CompressionType.Gzip;
            }

            if (acceptEncodingHeader.IndexOf(Constants.DeflateEncodingHeaderValue, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return CompressionType.Deflate;
            }

            return CompressionType.NoCompression;
        }
    }
}
