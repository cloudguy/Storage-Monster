using System;
using CloudBin.Core.Utilities;

namespace CloudBin.Web.Core.Configuration
{
    public sealed class XmlAuthenticationConfiguration : IAuthenticationConfiguration
    {
        private static readonly Lazy<AuthenticationConfigurationSection> ConfigSection = new Lazy<AuthenticationConfigurationSection>(() =>
        {
            return (AuthenticationConfigurationSection)System.Configuration.ConfigurationManager.GetSection(AuthenticationConfigurationSection.SectionLocation);
        }, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        private static byte[] _encryptionKey;
        private static byte[] _validationKey;
        private static readonly object Locker = new object();

        private void InitializeKeys()
        {
            lock (Locker)
            {
                if (_encryptionKey == null)
                {
                    _encryptionKey = ConfigSection.Value.EncryptionKey.GetByteArrayFromHexString();
                }
                if (_validationKey == null)
                {
                    _validationKey = ConfigSection.Value.ValidationKey.GetByteArrayFromHexString();
                }
            }
        }

        string IAuthenticationConfiguration.CookieName
        {
            get { return ConfigSection.Value.CookieName; }
        }

        string IAuthenticationConfiguration.SignInUrl
        {
            get { return ConfigSection.Value.SignInUrl; }
        }

        byte[] IAuthenticationConfiguration.EncryptionKey
        {
            get
            {
                if (_encryptionKey == null)
                {
                    InitializeKeys();
                }
                return _encryptionKey;
            }
        }

        byte[] IAuthenticationConfiguration.ValidationKey
        {
            get
            {
                if (_validationKey == null)
                {
                    InitializeKeys();
                }
                return _validationKey;
            }
        }

        bool IAuthenticationConfiguration.SlideExpire
        {
            get { return ConfigSection.Value.SlideExpire; }
        }

        bool IAuthenticationConfiguration.RequireSSL
        {
            get { return ConfigSection.Value.RequireSSL; }
        }

        bool IAuthenticationConfiguration.AllowMultipleSessions
        {
            get { return ConfigSection.Value.AllowMultipleSessions; }
        }

        string IAuthenticationConfiguration.EncryptionAlgorithm
        {
            get { return ConfigSection.Value.EncryptionAlgorithm; }
        }

        string IAuthenticationConfiguration.ValidationAlgorithm
        {
            get { return ConfigSection.Value.ValidationAlgorithm; }
        }

        bool IAuthenticationConfiguration.DoNotAuthenticateScriptAndContent
        {
            get { return ConfigSection.Value.DoNotAuthenticateScriptAndContent; }
        }

        TimeSpan IAuthenticationConfiguration.SessionTimeout
        {
            get
            {
                if (ConfigSection.Value.SessionTimeout <= 0)
                {
                    throw new InvalidOperationException("Session timeout must not be less than or equal to zero");
                }
                return TimeSpan.FromMinutes(ConfigSection.Value.SessionTimeout);
            }
        }
    }
}
