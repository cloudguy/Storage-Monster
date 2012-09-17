using System;
using System.Globalization;
using System.Web.Configuration;
using System.Web.Security;
using System.Configuration;

namespace StorageMonster.Web.Services.Configuration
{
    internal sealed class WebConfiguration : IWebConfiguration
    {
        public const string AuthenticationExpirationConfigKey = "LoginTimeout";
        public const int DefaultAuthenticationExpiration = 30;

        public const string AuthenticationCookieNameKey = "LoginCookieName";
        public const string DefaultAuthenticationCookieName = "sm_auth";

        public const string LocaleCookieNameKey = "LocaleCookieName";
        public const string DefaultLocaleCookieName = "sm_locale";

        public const string LocaleCookieTimeoutKey = "LocaleCookieTimeout";
        public const int DefaultLocaleCookieTimeout = 600000;
        
        public const string AuthenticationSlidingExpirationConfigKey = "UseSlidingExpiration";
        public const bool DefaultAuthenticationSlidingExpiration = true;

        public const string AllowMultipleLogonsConfigKey = "AllowMultipleLogons";
        public const bool DefaultAllowMultipleLogons = false;

        public const string MinPasswordLengthConfigKey = "MinPasswordLength";
        public const int DefaultMinPasswordLength = 6;

        public const string RunSweeperConfigKey = "RunSweeper";
        public const bool DefaultRunSweeperConfig = true;

        public const string SweeperTimeoutConfigKey = "SweeperTimeout";
        public const int DefaultSweeperTimeout = 30;

        public const string ResetPasswordRequestExpirationConfigKey = "ResetPasswordRequestExpiration";
        public const int DefaultResetPasswordRequestExpiration = 30;

        public const string RestorePasswordMailFromConfigKey = "RestorePasswordMailFrom";
        public const string DefaultRestorePasswordMailFrom = "do-not-reply@storage-monster.com";

        public const string SiteUrlConfigKey = "SiteUrl";
        public const string DefaultsiteUrl = "";

        public const string AutoDetectSiteUrlConfigKey = "AutoDetectSiteUrl";
        public const bool DefaultautoDetectSiteUrl = false;
               

        private object _initObject;
        private readonly object _lock = new object();

       

        public void Initialize()
        {
            int minutes = ParsePositiveInt(AuthenticationExpirationConfigKey, DefaultAuthenticationExpiration);
            _authenticationExpiration = new TimeSpan(0, minutes, 0);
            //------------------------------
            _authenticationCookieName = ParseString(AuthenticationCookieNameKey, DefaultAuthenticationCookieName);
            //------------------------------
            _localeCookieName = ParseString(LocaleCookieNameKey, DefaultLocaleCookieName);
            //------------------------------
            minutes = ParsePositiveInt(LocaleCookieTimeoutKey, DefaultLocaleCookieTimeout);
            _localeCookieTimeout = new TimeSpan(0, minutes, 0);
            //------------------------------
            _minPasswordLength = ParsePositiveInt(MinPasswordLengthConfigKey, DefaultMinPasswordLength);
            //------------------------------
            _authenticationSlidingExpiration = ParseBool(AuthenticationSlidingExpirationConfigKey, DefaultAuthenticationSlidingExpiration);
            //------------------------------
            _allowMultipleLogons = ParseBool(AllowMultipleLogonsConfigKey, DefaultAllowMultipleLogons);
            //------------------------------
            _runSweeper = ParseBool(RunSweeperConfigKey, DefaultRunSweeperConfig);
            //------------------------------
            _restorePasswordMailFrom = ParseString(RestorePasswordMailFromConfigKey, DefaultRestorePasswordMailFrom);
            //------------------------------
            minutes = ParsePositiveInt(ResetPasswordRequestExpirationConfigKey, DefaultResetPasswordRequestExpiration);
            _resetPasswordRequestExpiration = new TimeSpan(0, minutes, 0);
            //------------------------------
            _siteUrl = ParseString(SiteUrlConfigKey, DefaultsiteUrl);
            _autoDetectSiteUrl = ParseBool(AutoDetectSiteUrlConfigKey, DefaultautoDetectSiteUrl);
            //------------------------------
            if (_runSweeper)
            {
                minutes = ParsePositiveInt(SweeperTimeoutConfigKey, DefaultSweeperTimeout);
                _sweeperTimeout = new TimeSpan(0, minutes, 0);
            }
            _initObject = new object();
        }

        private T SafeGet<T>(ref T value)
        {
            if (_initObject == null)
            {
                lock (_lock)
                {
                    if (_initObject == null)
                        Initialize();
                }
            }
            return value;
        }

        private static int ParsePositiveInt(string configSection, int defaultValue)
        {
            string sValue = WebConfigurationManager.AppSettings[configSection];
            if (sValue != null)
            {
                int minutes;
                bool parseResult = int.TryParse(sValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out minutes);
                if (!parseResult || minutes <= 0)
                    throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Configuration option {0} is invalid", configSection));

                return minutes;
            }
            return defaultValue;
        }

        private static string ParseString(string configSection, string defaultValue)
        {
            string sValue = WebConfigurationManager.AppSettings[configSection];
            if (sValue != null)
            {
                if (sValue == string.Empty)
                    throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "Configuration option {0} is invalid", configSection));

                return sValue;
            }
            return defaultValue;
        }

        private static bool ParseBool(string configSection, bool defaultValue)
        {
            string sValue = WebConfigurationManager.AppSettings[configSection];
            if (sValue != null)
            {
                return Convert.ToBoolean(sValue);
            }
            return defaultValue;
        }

        private TimeSpan _authenticationExpiration;
        private string _authenticationCookieName;
        private bool _allowMultipleLogons;        
        private bool _authenticationSlidingExpiration;
        private bool _runSweeper;
        private TimeSpan _sweeperTimeout;
        private TimeSpan _resetPasswordRequestExpiration;
        private string _restorePasswordMailFrom;
        private string _localeCookieName;
        private TimeSpan _localeCookieTimeout;
        private string _siteUrl;
        private bool _autoDetectSiteUrl;
        private int _minPasswordLength;

        public TimeSpan AuthenticationExpiration { get { return SafeGet(ref _authenticationExpiration); } }
        public String AuthenticationCookieName { get { return SafeGet(ref _authenticationCookieName); } }
        public bool AuthenticationSlidingExpiration { get { return SafeGet(ref _authenticationSlidingExpiration); } }
        public bool AllowMultipleLogons { get { return SafeGet(ref _allowMultipleLogons); } }
        public bool RunSweeper { get { return SafeGet(ref _runSweeper); } }
        public TimeSpan SweeperTimeout { get { return SafeGet(ref _sweeperTimeout); } }
        public string RestorePasswordMailFrom { get { return SafeGet(ref _restorePasswordMailFrom); } }
        public TimeSpan ResetPasswordRequestExpiration { get { return SafeGet(ref _resetPasswordRequestExpiration); } }
        public string LocaleCookieName { get { return SafeGet(ref _localeCookieName); } }
        public TimeSpan LocaleCookieTimeout { get { return SafeGet(ref _localeCookieTimeout); } }
        public string SiteUrl { get { return SafeGet(ref _siteUrl); } }
        public bool AutoDetectSiteUrl { get { return SafeGet(ref _autoDetectSiteUrl); } }
        public int MinPasswordLength { get { return SafeGet(ref _minPasswordLength); } }
    }
}
