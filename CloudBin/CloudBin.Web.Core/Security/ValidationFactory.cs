using System.Security.Cryptography;

namespace CloudBin.Web.Core.Security
{
    internal class ValidationFactory
    {
        internal static ICookieValidation Create(string algorithm, byte[] secretKey)
        {
            return new KeyedHashValidation(KeyedHashAlgorithm.Create(algorithm), secretKey);
        }
    }
}
