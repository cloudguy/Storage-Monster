using CloudBin.Core;
using CloudBin.Core.Utilities;
using CloudBin.Web.Core.Configuration;
using CloudBin.Web.Core.Security;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Web;

namespace CloudBin.Web.Core.Tracking
{
    public sealed class CookieTrackingService : ITrackingService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IWebConfiguration _webConfiguration;
        private readonly IAuthenticationConfiguration _authenticationConfiguration;
        private const string DirtyKey = "tracking_dirty";
        private const string TrackingKeysKey = "tracking_keys";
        
        public CookieTrackingService(IWebConfiguration webConfiguration, IAuthenticationConfiguration authenticationConfiguration)
        {
            _webConfiguration = webConfiguration;
            _authenticationConfiguration = authenticationConfiguration;
        }

        string ITrackingService.GetTrackedValue(string key)
        {
            Verify.NotNull(()=>key);
            IDictionary<string, string> trackingKeys = GetTrackingKeys();
            return trackingKeys[key];
        }

        bool ITrackingService.TryGetTrackedValue(string key, out string value)
        {
            Verify.NotNull(() => key);
            IDictionary<string, string> trackingKeys = GetTrackingKeys();
            return trackingKeys.TryGetValue(key, out value);
        }

        void ITrackingService.SetTrackedValue(string key, string value)
        {
            Verify.NotNullOrWhiteSpace(()=>key);
            IDictionary<string, string> trackingKeys = GetTrackingKeys();
            trackingKeys[key] = value;
            SetDirty();
        }

        string ITrackingService.SerializeTrackedData()
        {
            IDictionary<string, string> trackingKeys = GetTrackingKeys();
            using (CookieProtector protector = new CookieProtector(_authenticationConfiguration))
            {
                TrackingCookie trackingCookie =new TrackingCookie(trackingKeys);
                return protector.Protect(trackingCookie.Serialize());
            }
        }

        bool ITrackingService.Dirty
        {
            get { return RequestContext.Current.LookUpValue(DirtyKey, () => false); }
        }

        private void SetDirty()
        {
            RequestContext.Current.SetValue(DirtyKey, true);
        }

        private IDictionary<string, string> GetTrackingKeys()
        {
            return RequestContext.Current.LookUpValue(TrackingKeysKey, () =>
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(CookieHelper.GetCookieName(_webConfiguration.TrackingCookieName));
                if (cookie == null)
                {
                    return new Dictionary<string, string>(StringComparer.Ordinal);
                }
                using (CookieProtector protector = new CookieProtector(_authenticationConfiguration))
                {
                    TrackingCookie trackingCookie;
                    if (!ValidateTrackingCookie(protector, cookie, out trackingCookie))
                    {
                        return new Dictionary<string, string>(StringComparer.Ordinal);
                    }

                    return trackingCookie.TrackingData ?? new Dictionary<string, string>(StringComparer.Ordinal);
                }
            });
        }

        private bool ValidateTrackingCookie(CookieProtector protector, HttpCookie httpCookie, out TrackingCookie trackingCookie)
        {
            trackingCookie = null;
            try
            {
                byte[] data;
                if (!protector.Validate(httpCookie.Value, out data))
                {
                    return false;
                }
                trackingCookie = TrackingCookie.Deserialize(data);
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("Tracking cookie validation error", ex);
                return false;
            }
        }
    }
}
