using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using StorageMonster.Services.Security;

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