using System;
using System.Runtime.Serialization;

namespace StorageMonster.Common
{
    [Serializable]
    public class DeliveryException : MonsterException
    {
        public DeliveryException()
        {
        }

        public DeliveryException(string message)
            : base(message)
        {
        }

        public DeliveryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DeliveryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

