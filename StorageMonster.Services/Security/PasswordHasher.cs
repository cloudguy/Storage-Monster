using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StorageMonster.Services.Security
{
    public static class PasswordHasher
    {
        private const string SaltChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const int SaltLength = 8;

        public static string EncryptPassword(string password)
        {
            string salt = GenerateSalt(SaltLength);
            return EncryptPassword(password, salt);
        }

        public static string EncryptPassword(string password, string salt)
        {
            string passwordAndSalt = salt + password;
            var md5Hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(passwordAndSalt));
            var sha512Hash = SHA512.Create().ComputeHash(md5Hash);
            return salt + Convert.ToBase64String(sha512Hash);
        }

        public static string GetSaltFromHash(string hash)
        {
            if (string.IsNullOrEmpty(hash) || hash.Length < SaltLength)
                throw new StorageMonsterSecurityException("Password hash is invalid");

            return hash.Substring(0, SaltLength);
        }



        public static string GenerateSalt(int length)
        {
            char[] chars = SaltChars.ToCharArray();

            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            int size = length;

            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);

            foreach (byte b in data)
                result.Append(chars[b%(chars.Length - 1)]);

            return result.ToString();
        }
    }
}
