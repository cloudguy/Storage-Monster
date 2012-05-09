using System;
using System.Runtime.Serialization;

namespace StorageMonster.Common
{
    [Serializable]
    public class StorageMonsterException : Exception
    {
        public StorageMonsterException()
        {
        }

        public StorageMonsterException(string message)
            : base(message)
        {
        }

        public StorageMonsterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected StorageMonsterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
