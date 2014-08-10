using System.Security.Cryptography;

namespace CloudBin.Web.Core.Security
{
    internal static class EncryptionFactory
    {
        internal static ICookieEncryption Create(string algorithm, byte[] secretKey)
        {
            return new SymmetricEncryption(SymmetricAlgorithm.Create(algorithm), secretKey);
        }
    }
}
