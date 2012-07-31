using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace StorageMonster.Common
{
    [Serializable]
    public class MonsterFrontException : MonsterException
    {
        public MonsterFrontException()
        {
        }

        public MonsterFrontException(string message)
            : base(message)
        {
        }

        public MonsterFrontException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MonsterFrontException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
