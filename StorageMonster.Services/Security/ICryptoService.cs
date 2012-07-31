using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Services.Security
{
    public interface ICryptoService
    {
        string EncryptString(string plainText, string sharedSecret, byte[] salt);
        string DecryptString(string cipherText, string sharedSecret, byte[] salt);
    }
}
