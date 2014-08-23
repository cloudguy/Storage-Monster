using CloudBin.Web.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudBin.Web.Core.Tracking
{
    public sealed class TrackingHttpModule : IHttpModule
    {
        private static readonly Lazy<ITrackingService> TrackingServiceLazy = new Lazy<ITrackingService>(() =>
        {
            return (ITrackingService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(ITrackingService));
        });
        private static readonly Lazy<IWebConfiguration> WebConfigurationLazy = new Lazy<IWebConfiguration>(() =>
        {
            return (IWebConfiguration)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IWebConfiguration));
        });
        private static readonly object Locker = new object();
        private volatile Func<HttpContext, bool>[] _staticContentCheckerInternal;

        void IHttpModule.Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += context_PreSendRequestHeaders;
        }

        private void context_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            if (!application.Response.IsClientConnected)
            {
                return;
            }
            if (StaticContentCheckers.Any(checker => checker(application.Context)))
            {
                return;
            }
            if (!TrackingServiceLazy.Value.Dirty)
            {
                return;
            }

            string serializedTrackingData = TrackingServiceLazy.Value.SerializeTrackedData();
            HttpCookie httpCookie = new HttpCookie(CookieHelper.GetCookieName(WebConfigurationLazy.Value.TrackingCookieName), serializedTrackingData);
            httpCookie.HttpOnly = true;
            httpCookie.Expires = DateTime.UtcNow.AddDays(100);
            application.Response.Cookies.Add(httpCookie);
        }

        void IHttpModule.Dispose()
        {
        }

        private IEnumerable<Func<HttpContext, bool>> StaticContentCheckers
        {
            get
            {
                if (_staticContentCheckerInternal == null)
                {
                    lock (Locker)
                    {
                        if (_staticContentCheckerInternal == null)
                        {
                            _staticContentCheckerInternal = RequestCheckersFactory.CreateStaticContentCheckers(HttpContext.Current);
                        }
                    }
                }
                return _staticContentCheckerInternal;
            }
        }
    }
}
