using System;
using System.Security.Cryptography;
using System.Text;
using StorageMonster.Services.Security;

namespace StorageMonster.Services.Facade
{
    public class PasswordHasher : IPasswordHasher
    {
        private const string SaltChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const int SaltLength = 8;

        public string EncryptPassword(string password)
        {
            string salt = GenerateSalt(SaltLength);
            return EncryptPassword(password, salt);
        }

        public string EncryptPassword(string password, string salt)
        {
            string passwordAndSalt = salt + password;
            using (var md5 = MD5.Create())
            using (var sha512 = SHA512.Create())
            {
                var md5Hash = md5.ComputeHash(Encoding.UTF8.GetBytes(passwordAndSalt));
                var sha512Hash = sha512.ComputeHash(md5Hash);
                return salt + Convert.ToBase64String(sha512Hash);
            }
        }

        public string GetSaltFromHash(string hash)
        {
            if (string.IsNullOrEmpty(hash) || hash.Length < SaltLength)
                throw new MonsterSecurityException("Password hash is invalid");

            return hash.Substring(0, SaltLength);
        }

        private static string GenerateSalt(int length)
        {
#warning rewrite
            char[] chars = SaltChars.ToCharArray();

            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);

                data = new byte[length];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(length);

            foreach (byte b in data)
                result.Append(chars[b%(chars.Length - 1)]);

            return result.ToString();
        }
    }
}
