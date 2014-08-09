using System;

namespace CloudBin.Web.Utilities.Security
{
    internal interface ICookieEncryption : IDisposable
    {
        byte[] Encrypt(byte[] valueBytes, byte[] initializationVector = null);
        byte[] Decrypt(byte[] encryptedValue, byte[] initializationVector = null);
    }
}
