using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Services.Security
{
    public interface ISecurityConfiguration
    {
        string EncryptionKey { get; }
        byte[] EncryptionSalt { get; }
    }
}
