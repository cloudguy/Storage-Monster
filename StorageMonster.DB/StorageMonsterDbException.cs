using System;
using System.Runtime.Serialization;
using StorageMonster.Common;

namespace StorageMonster.DB
{
    [Serializable]
    public class StorageMonsterDbException : StorageMonsterException
    {
        public StorageMonsterDbException()
        {
        }

        public StorageMonsterDbException(string message)
            : base(message)
        {
        }

        public StorageMonsterDbException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected StorageMonsterDbException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
