using System;
using System.Runtime.Serialization;


namespace StorageMonster.Common
{
    [Serializable]
    public class ObjectNotExistsException : MonsterException
    {
        public ObjectNotExistsException()
        {
        }

        public ObjectNotExistsException(string message)
            : base(message)
        {
        }

        public ObjectNotExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ObjectNotExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
