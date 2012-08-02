using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using StorageMonster.Common;

namespace StorageMonster.Web.Services
{
    public class MailService : IMailService
    {
        protected static void ExecuteMailAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            try
            {
                action();
            }
            catch (Exception ex)
            {
                throw new DeliveryException("Mail delivery failed", ex);
            }
        }

        public void SendMail(string subject, string body, string from, IEnumerable<string> recipients)
        {
            if (recipients == null)
                throw new ArgumentNullException("recipients");

            if (string.IsNullOrEmpty(from))
                throw new ArgumentNullException("from");

            ExecuteMailAction(() =>
            {
                var mailMessage = new MailMessage();
                foreach (var recipient in recipients)
                {
                    mailMessage.To.Add(recipient);
                }

                mailMessage.From = new MailAddress(from);
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                var smtpClient = new SmtpClient();
                smtpClient.Send(mailMessage);
            });
        }

        public void SendMail(string subject, string body, string from, string recipient)
        {
            if (string.IsNullOrEmpty(recipient) == null)
                throw new ArgumentNullException("recipient");

            if (string.IsNullOrEmpty(from))
                throw new ArgumentNullException("from");

            ExecuteMailAction(() =>
            {
                var mailMessage = new MailMessage();
                mailMessage.To.Add(recipient);

                mailMessage.From = new MailAddress(from);
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                var smtpClient = new SmtpClient();
                smtpClient.Send(mailMessage);
            });
        }
    }
}