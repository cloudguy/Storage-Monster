using CloudBin.Core;
using System;
using System.Web;

namespace CloudBin.Web.Core.Globalization
{
    public class LocaleHttpModule : IHttpModule
    {
        private static readonly Lazy<ILocaleProvider> LocaleProviderLazy = new Lazy<ILocaleProvider>(() =>
        {
            return (ILocaleProvider)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(ILocaleProvider));
        });

        void IHttpModule.Init(HttpApplication context)
        {
            context.AuthorizeRequest += context_AuthorizeRequest;
        }

        void context_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication) sender;
            Locale locale = LocaleGettersChain.GetLocale(application);
            LocaleProviderLazy.Value.SetThreadLocale(locale);
        }

        void IHttpModule.Dispose()
        {
        }
    }
}
