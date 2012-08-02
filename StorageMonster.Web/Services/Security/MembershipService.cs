using System;
using System.Web.Security;
using StorageMonster.Domain;
using System.Globalization;
using StorageMonster.Common;
using StorageMonster.Services;
using StorageMonster.Web.Services.Configuration;
using StorageMonster.Web.Properties;
using System.Collections.Generic;
using StorageMonster.Services.Security;

namespace StorageMonster.Web.Services.Security
{
    public class MembershipService : IMembershipService
    {
        protected IUserService UserService { get; set; }
        protected ITemplateEngine TemplateEngine { get; set; }
        protected IMailService MailService { get; set; }
        protected IWebConfiguration WebConfiguration { get; set; }
        protected IPasswordHasher PasswordHasher { get; set; }  

        public MembershipService(IUserService userService, 
            ITemplateEngine templateEngine,
            IMailService mailService,
            IWebConfiguration webConfiguration,
            IPasswordHasher passwordHasher)
        {
            UserService = userService;
            TemplateEngine = templateEngine;
            MailService = mailService;
            WebConfiguration = webConfiguration;
            PasswordHasher = passwordHasher;

            if (Membership.Provider is MonsterMembershipProvider)
                Provider = Membership.Provider;
            else
                throw new ArgumentException("Membership provider not supported");
        }

        protected readonly MembershipProvider Provider;       

        public int MinPasswordLength
        {
            get
            {
                return Provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string email, string password)
        {
            return Provider.ValidateUser(email, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string userEmail, string locale)
        {
            MembershipData membershipData = new MembershipData
                {
                    Locale = locale
                };

            MembershipCreateStatus status;
            Provider.CreateUser(userName, password, userEmail, null, null, true, membershipData, out status);
            return status;
        }

        public User UpdateUser(int userId, string userName, string locale, DateTime stamp)
        {
            User user = UserService.Load(userId);
            if (user == null)
                throw new ObjectNotExistsException(string.Format(CultureInfo.InvariantCulture, "Error updating user {0}, user not found", userId));

            user.Stamp = stamp;
            user.Name = userName;
            user.Locale = locale;
            UserService.UpdateUser(user);
            return user;
        }

        public void RequestPasswordReset(string email, string siteurl, Func<string, string> urlGenerator)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");

            if (string.IsNullOrEmpty(siteurl))
                throw new ArgumentNullException("siteurl");

            if (urlGenerator == null)
                throw new ArgumentNullException("urlGenerator");

            User user = UserService.GetUserByEmail(email);
            if (user == null)
                throw new ObjectNotExistsException(string.Format(CultureInfo.InvariantCulture, "User with email {0} not found", email));

            DateTime expiration = DateTime.UtcNow.Add(WebConfiguration.ResetPasswordRequestExpiration);

            ResetPasswordRequest request = UserService.CreatePasswordResetRequestForUser(user, expiration);
#warning in timezone
            string link = urlGenerator(request.Token);
            var templateData = new Dictionary<string, object>
            {
                { "link", link },
                { "siteurl", siteurl },
                { "username", user.Name },
                { "expire", new DateTimeOffset(request.Expiration).ToString("dd.MM.yyyy HH:mm zzz") }
            };
            string emailBody = TemplateEngine.TransformTemplate(templateData, MailResources.RestorePasswordMailBody);
            string emailSubject = TemplateEngine.TransformTemplate(templateData, MailResources.RestorePasswordMailSubject);



            MailService.SendMail(emailSubject, emailBody, WebConfiguration.RestorePasswordMailFrom, user.Email);
        }

        public ResetPasswordRequest GetActivePasswordResetRequestByToken(string token)
        {
            TimeSpan resetPasswdTimeout = WebConfiguration.ResetPasswordRequestExpiration;
            DateTime lowerTimeLimit = DateTime.UtcNow.Subtract(resetPasswdTimeout);

            return UserService.GetActivePasswordResetRequestByToken(token);
        }

        public void ChangePassword(string resetToken, string newPassword)
        {
            if (string.IsNullOrEmpty(resetToken))
                throw new InvalidPasswordTokenException("Password reset token is empty");

            ResetPasswordRequest request = this.GetActivePasswordResetRequestByToken(resetToken);
            if (request == null)
                throw new InvalidPasswordTokenException(string.Format(CultureInfo.InvariantCulture, "Password reset token {0} not found or expired", resetToken));
#warning check pass length
            User user = UserService.Load(request.UserId);
            if (user == null)
#warning make new exception for user
                throw new ObjectNotExistsException(string.Format(CultureInfo.InvariantCulture, "User with id {0} not exists", request.UserId));

            string passwordHash = PasswordHasher.EncryptPassword(newPassword);
            UserService.UpdateUser(user);
            UserService.DeleteResetPasswordRequest(request.Id);
        }

        public void ChangePassword(int userId, string newPassword, string oldPassword)
        {
        }
    }
}
