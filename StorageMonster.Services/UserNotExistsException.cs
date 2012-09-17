using System;
using System.Globalization;
using System.Runtime.Serialization;
using StorageMonster.Common;

namespace StorageMonster.Services
{
    [Serializable]
    public class UserNotExistsException : ObjectNotExistsException
    {
        public UserNotExistsException(int userId)
            : base(string.Format(CultureInfo.InvariantCulture, "User with id {0} does not exists", userId))
        {
        }

        public UserNotExistsException()
        {
        }

        public UserNotExistsException(string message)
            : base(message)
        {
        }

        public UserNotExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UserNotExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
