using System;
using System.Globalization;
using System.Runtime.Serialization;
using StorageMonster.Common;

namespace StorageMonster.Services
{
    public class StaleUserException : StaleObjectException
    {
        public StaleUserException(int userId)
            : base(string.Format(CultureInfo.InvariantCulture, "User with id {0} stalled", userId))
        {
        }

        public StaleUserException()
        {
        }

        public StaleUserException(string message)
            : base(message)
        {
        }

        public StaleUserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected StaleUserException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
