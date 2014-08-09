using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudBin.Web.Utilities.Configuration;

namespace CloudBin.Web.Utilities.Security
{
    internal sealed class CookieProtector : IDisposable
    {
        private readonly ICookieEncryption _cookieEncryption;
        private readonly ICookieValidation _cookieValidation;

        internal CookieProtector(IAuthenticationConfiguration configuration)
        {
            _cookieEncryption = EncryptionFactory.Create(configuration.EncryptionAlgorithm, configuration.EncryptionKey);
            _cookieValidation = ValidationFactory.Create(configuration.ValidationAlgorithm, configuration.ValidationKey);
        }

        internal bool Validate(string cookie, out byte[] data)
        {
            data = null;

            var versionedCookieData = Convert.FromBase64String(cookie);
            if (versionedCookieData.Length == 0 || versionedCookieData[0] != 0)
                return false;

            var cookieData = new byte[versionedCookieData.Length - 1];
            Buffer.BlockCopy(versionedCookieData, 1, cookieData, 0, cookieData.Length);

            if (!_cookieValidation.Validate(cookieData))
            {
                return false;
            }

            cookieData = _cookieValidation.StripSignature(cookieData);
            cookieData = _cookieEncryption.Decrypt(cookieData);

            data = cookieData;
            return true;
        }

        internal string Protect(byte[] data)
        {
            data = _cookieEncryption.Encrypt(data);
            data = _cookieValidation.Sign(data);
            var versionedData = new byte[data.Length + 1];
            Buffer.BlockCopy(data, 0, versionedData, 1, data.Length);
            return Convert.ToBase64String(versionedData);
        }

        void IDisposable.Dispose()
        {
            if (_cookieEncryption != null)
            {
                _cookieEncryption.Dispose();
            }
            if (_cookieValidation != null)
            {
                _cookieValidation.Dispose();
            }
        }
    }
}
