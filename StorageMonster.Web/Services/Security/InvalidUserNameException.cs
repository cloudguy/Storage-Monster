using StorageMonster.Services.Security;
using System;
using System.Runtime.Serialization;

namespace StorageMonster.Web.Services.Security
{
    [Serializable]
    public class InvalidUserNameException : MonsterSecurityException
    {
        public InvalidUserNameException()
        {
        }

        public InvalidUserNameException(string message)
            : base(message)
        {
        }

        public InvalidUserNameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidUserNameException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}