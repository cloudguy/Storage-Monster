using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using StorageMonster.Common;

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
