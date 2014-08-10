using System;
using System.IO;
using System.Security.Cryptography;

namespace CloudBin.Web.Core.Security
{
    internal sealed class SymmetricEncryption : ICookieEncryption
    {
        private readonly SymmetricAlgorithm _algorithm;
        private readonly byte[] _secretKey;

        internal SymmetricEncryption(SymmetricAlgorithm algorithm, byte[] secretKey)
        {
            _algorithm = algorithm;
            _secretKey = secretKey;
        }

        void IDisposable.Dispose()
        {
            _algorithm.Dispose();
        }


        byte[] ICookieEncryption.Encrypt(byte[] valueBytes, byte[] initializationVector)
        {
// ReSharper disable InconsistentNaming
            bool generateRandomIV = initializationVector == null;
// ReSharper restore InconsistentNaming
            if (generateRandomIV)
            {
                initializationVector = new byte[_algorithm.BlockSize / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(initializationVector);
                }
            }
            using (var output = new MemoryStream())
            {
                if (generateRandomIV)
                {
                    output.Write(initializationVector, 0, initializationVector.Length);
                }
                using (var cryptoOutput = new CryptoStream(output, _algorithm.CreateEncryptor(_secretKey, initializationVector), CryptoStreamMode.Write))
                {
                    cryptoOutput.Write(valueBytes, 0, valueBytes.Length);
                }

                return output.ToArray();
            }
        }

        byte[] ICookieEncryption.Decrypt(byte[] encryptedValue, byte[] initializationVector)
        {
            int dataOffset = 0;
            if (initializationVector == null)
            {
                initializationVector = new byte[_algorithm.BlockSize / 8];
                Buffer.BlockCopy(encryptedValue, 0, initializationVector, 0, initializationVector.Length);
                dataOffset = initializationVector.Length;
            }
            using (var output = new MemoryStream())
            {
                using (var cryptoOutput = new CryptoStream(output, _algorithm.CreateDecryptor(_secretKey, initializationVector), CryptoStreamMode.Write))
                {
                    cryptoOutput.Write(encryptedValue, dataOffset, encryptedValue.Length - dataOffset);
                }

                return output.ToArray();
            }
        }
    }
}
