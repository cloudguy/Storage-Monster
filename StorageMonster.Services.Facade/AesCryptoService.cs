using System;
using System.IO;
using System.Security.Cryptography;
using StorageMonster.Services.Security;

namespace StorageMonster.Services.Facade
{
    public class AesCryptoService : ICryptoService
    {
        public string EncryptString(string plainText, string sharedSecret, byte[] salt)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr;                       
            RijndaelManaged aesAlg = null;  

            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, salt);               
                aesAlg = new RijndaelManaged();
                aesAlg.KeySize = 256;
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);               
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);               
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {               
                if (aesAlg != null)
                    aesAlg.Clear();
            }                       
            return outStr;
        }

        public string DecryptString(string cipherText, string sharedSecret, byte[] salt)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");
          
            RijndaelManaged aesAlg = null;           
            string plaintext;

            try
            {               
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, salt);
     
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    aesAlg = new RijndaelManaged();
                    aesAlg.KeySize = 256;
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);                   
                    aesAlg.IV = ReadByteArray(msDecrypt);                    
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new MonsterSecurityException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new MonsterSecurityException("Did not read byte array properly");
            }

            return buffer;
        }
    }

}

