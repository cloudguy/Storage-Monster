using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace StorageMonster.Web.Services.Mail
{
    public class MailDeliveryService : IMessageDeliveryService
    {
        public void SendMessage(string subject, string body, string from, IEnumerable<string> recipients)
        {
            if (recipients == null)
                throw new ArgumentNullException("recipients");

            if (string.IsNullOrEmpty(from))
                throw new ArgumentNullException("from");


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
        }

        public void SendMessage(string subject, string body, string from, string recipient)
        {
            if (string.IsNullOrEmpty(recipient))
                throw new ArgumentNullException("recipient");

            if (string.IsNullOrEmpty(from))
                throw new ArgumentNullException("from");


            var mailMessage = new MailMessage();
            mailMessage.To.Add(recipient);

            mailMessage.From = new MailAddress(from);
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            var smtpClient = new SmtpClient();
            smtpClient.Send(mailMessage);
        }
    }
}