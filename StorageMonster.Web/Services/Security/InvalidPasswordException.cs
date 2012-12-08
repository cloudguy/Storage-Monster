using StorageMonster.Services.Security;
using System;
using System.Runtime.Serialization;

namespace StorageMonster.Web.Services.Security
{
    [Serializable]
    public class InvalidPasswordException : MonsterSecurityException
    {
        public InvalidPasswordException()
        {
        }

        public InvalidPasswordException(string message)
            : base(message)
        {
        }

        public InvalidPasswordException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidPasswordException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}