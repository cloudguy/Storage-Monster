using System;
using System.Runtime.Serialization;

namespace StorageMonster.Common
{
    [Serializable]
    public class MonsterException : Exception
    {
        public MonsterException()
        {
        }

        public MonsterException(string message)
            : base(message)
        {
        }

        public MonsterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MonsterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
