using System;
using System.Runtime.Serialization;
using StorageMonster.Common;

namespace StorageMonster.Services.Security
{
    [Serializable]
    public class MonsterSecurityException : MonsterException
    {
        public MonsterSecurityException()
        {
        }

        public MonsterSecurityException(string message)
            : base(message)
        {
        }

        public MonsterSecurityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MonsterSecurityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
