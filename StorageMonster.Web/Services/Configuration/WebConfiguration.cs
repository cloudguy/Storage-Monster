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

        public const string AuthenticationnCookieNameKey = "LoginCookieName";
        public const string DefaultAuthenticationnCookieName = "sm_auth";

        public const string AuthenticationnLoginUrlConfigKey = "LoginUrl";
        public const string DefaultAuthenticationnLoginUrl = "~/Account/LogOn";

        public const string AuthenticationnSlidingExpirationConfigKey = "UseSlidingExpiration";
        public const bool DefaultAuthenticationnSlidingExpiration = true;

        public const string AllowMultipleLogonsConfigKey = "AllowMultipleLogons";
        public const bool DefaultAllowMultipleLogons = false;
        
        public const string RunSweeperConfigKey = "RunSweeper";
        public const bool DefaultRunSweeperConfig = true;

        public const string SweeperTimeoutConfigKey = "SweeperTimeout";
        public const int DefaultSweeperTimeout = 30;

        public const string ResetPasswordRequestExpirationConfigKey = "ResetPasswordRequestExpiration";
        public const int DefaultResetPasswordRequestExpiration = 30;

        public const string RestorePasswordMailFromConfigKey = "RestorePasswordMailFrom";
        public const string DefaultRestorePasswordMailFrom = "do-not-reply@storage-monster.com";
               

        private object _initObject;
        private readonly object _lock = new object();

       

        public void Initialize()
        {
            int minutes = ParsePositiveInt(AuthenticationExpirationConfigKey, DefaultAuthenticationExpiration);
            _authenticationExpiration = new TimeSpan(0, minutes, 0);
            //------------------------------
            _authenticationCookieName = ParseString(AuthenticationnCookieNameKey, DefaultAuthenticationnCookieName);
            //------------------------------
            _loginUrl = ParseString(AuthenticationnLoginUrlConfigKey, DefaultAuthenticationnLoginUrl);
            //------------------------------
            _authenticationSlidingExpiration = ParseBool(AuthenticationnSlidingExpirationConfigKey, DefaultAuthenticationnSlidingExpiration);
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
        private string _loginUrl;
        private bool _authenticationSlidingExpiration;
        private bool _runSweeper;
        private TimeSpan _sweeperTimeout;
        private TimeSpan _resetPasswordRequestExpiration;
        private string _restorePasswordMailFrom;       

        public TimeSpan AuthenticationExpiration { get { return SafeGet(ref _authenticationExpiration); } }
        public String AuthenticationCookiename { get { return SafeGet(ref _authenticationCookieName); } }
        public bool AuthenticationSlidingExpiration { get { return SafeGet(ref _authenticationSlidingExpiration); } }
        public string LoginUrl { get { return SafeGet(ref _loginUrl); } }
        public bool AllowMultipleLogons { get { return SafeGet(ref _allowMultipleLogons); } }
        public bool RunSweeper { get { return SafeGet(ref _runSweeper); } }
        public TimeSpan SweeperTimeout { get { return SafeGet(ref _sweeperTimeout); } }
        public string RestorePasswordMailFrom { get { return SafeGet(ref _restorePasswordMailFrom); } }
        public TimeSpan ResetPasswordRequestExpiration { get { return SafeGet(ref _resetPasswordRequestExpiration); } }
    }
}
