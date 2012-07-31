using System;
using System.Runtime.Serialization;
using StorageMonster.Common;

namespace StorageMonster.Database
{
    [Serializable]
    public class MonsterDbException : MonsterException
    {
        public MonsterDbException()
        {
        }

        public MonsterDbException(string message)
            : base(message)
        {
        }

        public MonsterDbException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MonsterDbException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
