using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Web.Services
{
    public interface IMessageDeliveryService
    {
        void SendMessage(string subject, string body, string from, IEnumerable<string> recipients);
        void SendMessage(string subject, string body, string from, string recipient);
    }
}
