using StorageMonster.Common;
using System;
using System.Runtime.Serialization;

namespace StorageMonster.Web.Services.Mail
{
    [Serializable]
    public class MailDeliveryException : DeliveryException
    {
        public MailDeliveryException()
        {
        }

        public MailDeliveryException(string message)
            : base(message)
        {
        }

        public MailDeliveryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MailDeliveryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}