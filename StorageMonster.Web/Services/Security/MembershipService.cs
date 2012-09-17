using System;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.Security;
using StorageMonster.Domain;
using System.Globalization;
using StorageMonster.Services;
using StorageMonster.Web.Services.Configuration;
using StorageMonster.Web.Properties;
using System.Collections.Generic;
using StorageMonster.Services.Security;
using Common.Logging;
using StorageMonster.Database;

namespace StorageMonster.Web.Services.Security
{
    public class MembershipService : IMembershipService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MembershipService));

        private static readonly Regex EmailValidationRegex = new Regex(Constants.EmailRegexp, RegexOptions.Compiled);
        private static readonly Regex UserNameValidationRegex = new Regex(Constants.UserNameRegexp, RegexOptions.Compiled);

        private readonly IUserService _userService;
        private readonly ITemplateEngine _templateEngine;
        private readonly IMessageDeliveryService _mailService;
        private readonly IWebConfiguration _webConfiguration;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILocaleProvider _localeProvider;
        private readonly ITimeZonesProvider _timeZonesProvider;
        private readonly IConnectionProvider _connectionProvider;

        public MembershipService(IUserService userService, 
            ITemplateEngine templateEngine,
            IMessageDeliveryService mailService,
            IWebConfiguration webConfiguration,
            IPasswordHasher passwordHasher,
            ILocaleProvider localeProvider,
            ITimeZonesProvider timeZonesProvider,
            IConnectionProvider connectionProvider)
        {
            _userService = userService;
            _templateEngine = templateEngine;
            _mailService = mailService;
            _webConfiguration = webConfiguration;
            _passwordHasher = passwordHasher;
            _localeProvider = localeProvider;
            _timeZonesProvider = timeZonesProvider;
            _connectionProvider = connectionProvider;
        }
            

        public int MinPasswordLength
        {
            get
            {
                return _webConfiguration.MinPasswordLength;
            }
        }

        public bool ValidateUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;


            var user = _userService.GetUserByEmail(email);
            if (user == null)
                return false;

            string salt;
            try
            {
                salt = _passwordHasher.GetSaltFromHash(user.Password);
            }
            catch (MonsterSecurityException ex)
            {
                Logger.ErrorFormat(CultureInfo.InvariantCulture, "User with email {0} has invalid password hash", ex, user.Email);
                return false;
            }

            string hash = _passwordHasher.EncryptPassword(password, salt);

            return user.Password.Equals(hash, StringComparison.InvariantCulture);
        }

        private static bool IsUserEmailValid(ref string email)
        {
            if (email == null)
                return false;
            email = email.Trim();
            return EmailValidationRegex.IsMatch(email) && email.Length <= 100;
        }

        private static bool IsUserNameValid(ref string userName)
        {
            if (userName == null)
                return false;
            userName = userName.Trim();
            return !string.IsNullOrEmpty(userName) && userName.Length <= 100 && UserNameValidationRegex.IsMatch(userName);
        }

        private bool IsUserPasswordValid(ref string userPassword)
        {
            if (userPassword == null)
                return false;

            return userPassword.Length >= _webConfiguration.MinPasswordLength && userPassword.Length <= 100;
        }

        public MembershipCreateStatus CreateUser(string email, string password, string userName, string locale, int timezone)
        {
            if (!IsUserEmailValid(ref email))
                return MembershipCreateStatus.InvalidEmail;
           
            if (!IsUserNameValid(ref userName))
                return MembershipCreateStatus.InvalidUserName;

            var userCheck = _userService.GetUserByEmail(email);
            if (userCheck != null)
                return MembershipCreateStatus.DuplicateEmail;

            if (!IsUserPasswordValid(ref password))
                return MembershipCreateStatus.InvalidPassword;

            User user = new User
                {
                    Email = email,
                    Name = userName,
                    Password = _passwordHasher.EncryptPassword(password),
                    Locale = _localeProvider.GetCultureByNameOrDefault(locale).ShortName,
                    TimeZone = _timeZonesProvider.GetTimeZoneByIdOrDefault(timezone).Id
                };

            try
            {
                _userService.Insert(user);
                _userService.CreateRoleForUser(user, Constants.RoleUser);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(CultureInfo.InvariantCulture, "Error creating user with email {0}", ex, user.Email);
                return MembershipCreateStatus.ProviderError;
            }           
            return MembershipCreateStatus.Success;
        }

        public User UpdateUser(int userId, string userName, string locale, int timezone, DateTime stamp, Identity identity)
        {
            User user = _userService.Load(userId);
            if (user == null)
                throw new UserNotExistsException(userId);

            if (string.IsNullOrEmpty(userName))
                throw new InvalidUserNameException("User name is empty");

            if (!IsUserNameValid(ref userName))
                throw new InvalidUserNameException(string.Format(CultureInfo.InvariantCulture, "User name {0} is invalid", userName));

            user.Stamp = stamp;
            user.Name = userName;
            user.Locale = _localeProvider.GetCultureByNameOrDefault(locale).ShortName;
            user.TimeZone = _timeZonesProvider.GetTimeZoneByIdOrDefault(timezone).Id;
            _userService.UpdateUser(user);

            LocaleData localeData = _localeProvider.GetCultureByNameOrDefault(user.Locale);
            _localeProvider.SetThreadLocale(localeData);
            identity.Name = user.Name;
            identity.Locale = localeData.ShortName;

            return user;
        }

        public void RequestPasswordReset(string email, string siteUrl, Func<string, string> resetUrlGenerator)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");

            if (string.IsNullOrEmpty(siteUrl))
                throw new ArgumentNullException("siteurl");

            if (resetUrlGenerator == null)
                throw new ArgumentNullException("resetUrlGenerator");

            User user = _userService.GetUserByEmail(email);

            if (user == null)
                throw new UserNotExistsException(string.Format(CultureInfo.InvariantCulture, "User with email {0} not found", email));

            DateTime expiration = DateTime.UtcNow.Add(_webConfiguration.ResetPasswordRequestExpiration);

            ResetPasswordRequest request = _userService.CreatePasswordResetRequestForUser(user, expiration);
            string link = resetUrlGenerator(request.Token);
            TimeZoneData timeZoneData = _timeZonesProvider.GetTimeZoneByIdOrDefault(user.TimeZone);
            var templateData = new Dictionary<string, object>
            {
                { "link", link },
                { "siteurl", siteUrl },
                { "username", user.Name },
                { "expire", new DateTimeOffset(request.Expiration).ToOffset(timeZoneData.Offset).ToString("dd.MM.yyyy HH:mm zzz") }
            };
            string emailBody = _templateEngine.TransformTemplate(templateData, MailResources.RestorePasswordMailBody);
            string emailSubject = _templateEngine.TransformTemplate(templateData, MailResources.RestorePasswordMailSubject);
            _mailService.SendMessage(emailSubject, emailBody, _webConfiguration.RestorePasswordMailFrom, user.Email);
        }

        public ResetPasswordRequest GetActivePasswordResetRequestByToken(string token)
        {
            return _userService.GetActivePasswordResetRequestByToken(token);
        }

        public void ChangePassword(string resetToken, string newPassword)
        {
            if (string.IsNullOrEmpty(resetToken))
                throw new InvalidPasswordTokenException("Password reset token is empty");

            if (string.IsNullOrEmpty(newPassword))
                throw new InvalidPasswordException("New password is empty");

            ResetPasswordRequest request = GetActivePasswordResetRequestByToken(resetToken);
            if (request == null)
                throw new InvalidPasswordTokenException(string.Format(CultureInfo.InvariantCulture, "Password reset token {0} not found or expired", resetToken));

            if (!IsUserPasswordValid(ref newPassword))
                throw new InvalidPasswordException(string.Format(CultureInfo.InvariantCulture, "Password {0} is invalid", newPassword));

            User user = _userService.Load(request.UserId);
            if (user == null)
                throw new UserNotExistsException(request.UserId);

            user.Password = _passwordHasher.EncryptPassword(newPassword);
            _userService.UpdateUser(user);
            _userService.DeleteResetPasswordRequest(request.Id);
        }

        public User ChangePassword(int userId, string newPassword, string oldPassword, DateTime userStamp)
        {
            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentNullException("newPassword");

            if (string.IsNullOrEmpty(oldPassword))
                throw new ArgumentNullException("oldPassword");

            if (!IsUserPasswordValid(ref newPassword))
                throw new InvalidPasswordException(string.Format(CultureInfo.InvariantCulture, "Password {0} is invalid", newPassword));

            User user = _userService.Load(userId);
            if (user == null)
                throw new UserNotExistsException(userId);

            if (!ValidatePasswords(user, oldPassword))
                throw new PasswordsMismatchException("Old password mismatch");
           

            user.Password = _passwordHasher.EncryptPassword(newPassword);
            user.Stamp = userStamp;
            _userService.UpdateUser(user);
            return user;
        }

        private bool ValidatePasswords(User user, string oldPasswordCheck)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            string salt;            
            try
            {
                salt = _passwordHasher.GetSaltFromHash(user.Password);
            }
            catch (MonsterSecurityException ex)
            {
                Logger.ErrorFormat(CultureInfo.InvariantCulture, "User with email {0} has invalid password hash", ex, user.Email);
                return false;
            }

            string checkHash = _passwordHasher.EncryptPassword(oldPasswordCheck, salt);

            return user.Password.Equals(checkHash, StringComparison.InvariantCulture);
        }
    }
}
