using System;
using System.Security.Cryptography;

namespace CloudBin.Web.Utilities.Security
{
    internal sealed class KeyedHashValidation : ICookieValidation
    {
        private readonly KeyedHashAlgorithm _algorithm;

        internal KeyedHashValidation(KeyedHashAlgorithm algorithm, byte[] secretKey)
        {
            _algorithm = algorithm;
            _algorithm.Key = secretKey;
        }

        void IDisposable.Dispose()
        {
            _algorithm.Dispose();
        }

        byte[] ICookieValidation.Sign(byte[] data)
        {
            var hashLength = _algorithm.HashSize / 8;
            var signedMessageLength = data.Length + hashLength;
            var signedMessage = new byte[signedMessageLength];
            Buffer.BlockCopy(data, 0, signedMessage, 0, data.Length);
            Buffer.BlockCopy(ComputeSignature(data), 0, signedMessage, data.Length, hashLength);
            return signedMessage;
        }

        byte[] ICookieValidation.StripSignature(byte[] signedMessage)
        {
            var hashLength = _algorithm.HashSize / 8;
            var dataLength = signedMessage.Length - hashLength;
            var data = new byte[dataLength];
            Buffer.BlockCopy(signedMessage, 0, data, 0, data.Length);
            return data;
        }

        bool ICookieValidation.Validate(byte[] signedMessage)
        {
            var hashLength = _algorithm.HashSize / 8;
            var dataLength = signedMessage.Length - hashLength;
            return Validate(signedMessage, dataLength);
        }

        private byte[] ComputeSignature(byte[] data)
        {
            return ComputeSignature(data, 0, data.Length);
        }

        private byte[] ComputeSignature(byte[] data, int offset, int count)
        {
            return _algorithm.ComputeHash(data, offset, count);
        }

        private bool Validate(byte[] signedMessage, int dataLength)
        {
            bool isValid = true;
            var validSignature = ComputeSignature(signedMessage, 0, dataLength);
            if (signedMessage.Length != dataLength + validSignature.Length)
            {
                return false;
            }
            for (int i = 0; i < validSignature.Length; i++)
            {
                if (i + dataLength >= signedMessage.Length)
                {
                    isValid = false;
                }
                if (signedMessage[i + dataLength] != validSignature[i])
                {
                    isValid = false;
                }
            }
            return isValid;
        }
    }
}
