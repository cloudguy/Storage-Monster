﻿using StorageMonster.Services.Security;
using System;
using System.Runtime.Serialization;

namespace StorageMonster.Web.Services.Security
{
    [Serializable]
    public class InvalidPasswordTokenException : MonsterSecurityException
    {
        public InvalidPasswordTokenException()
        {
        }

        public InvalidPasswordTokenException(string message)
            : base(message)
        {
        }

        public InvalidPasswordTokenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidPasswordTokenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

