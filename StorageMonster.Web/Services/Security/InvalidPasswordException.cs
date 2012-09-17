using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using StorageMonster.Services.Security;

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