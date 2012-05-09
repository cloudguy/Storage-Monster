using System;
using System.Runtime.Serialization;
using StorageMonster.Common;

namespace StorageMonster.Services.Security
{
    [Serializable]
    public class StorageMonsterSecurityException : StorageMonsterException
    {
        public StorageMonsterSecurityException()
        {
        }

        public StorageMonsterSecurityException(string message)
            : base(message)
        {
        }

        public StorageMonsterSecurityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected StorageMonsterSecurityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
