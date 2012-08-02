using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Web.Services
{
    public interface IMailService
    {
        void SendMail(string subject, string body, string from, IEnumerable<string> recipients);
        void SendMail(string subject, string body, string from, string recipient);
    }
}
