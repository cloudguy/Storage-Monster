using System;

using System.Runtime.Serialization;

namespace StorageMonster.Common
{
    [Serializable]
    public class StaleObjectException : MonsterException
    {
        public StaleObjectException()
        {
        }

        public StaleObjectException(string message)
            : base(message)
        {
        }

        public StaleObjectException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected StaleObjectException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
