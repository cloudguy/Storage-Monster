using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.Security;
using Common.Logging;
using StorageMonster.Database;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Services.Security;
using StorageMonster.Utilities;
using StorageMonster.Common;

namespace StorageMonster.Web.Services.Security
{
#warning remove provider?
    public class MonsterMembershipProvider : MembershipProvider
    {        private static readonly ILog Logger = LogManager.GetLogger(typeof(MonsterMembershipProvider));

        protected int MinRequiredPasswordLengthInternal = 6;
        protected Regex EmailValidationRegex = new Regex(Constants.EmailRegexp, RegexOptions.Compiled);
        protected Regex UserNameValidationRegex = new Regex(Constants.UserNameRegexp, RegexOptions.Compiled);

        public override int MinRequiredPasswordLength
        {
            get { return MinRequiredPasswordLengthInternal; }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            base.Initialize(name, config);
            string minPasswdLength = config["minRequiredPasswordLength"];

            if (!string.IsNullOrEmpty(minPasswdLength))
                MinRequiredPasswordLengthInternal = Int32.Parse(minPasswdLength, CultureInfo.InvariantCulture);
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (email == null)
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            string trimmedEmail = email.Trim();
            if (!EmailValidationRegex.IsMatch(trimmedEmail) || email.Length >100)
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            if (string.IsNullOrWhiteSpace(username) || username.Length > 100 || !UserNameValidationRegex.IsMatch(username))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }

            string trimmedUsername = username.Trim();

            var userService = IocContainer.Instance.Resolve<IUserService>();
            var user = userService.GetUserByEmail(trimmedEmail);
            if (user != null)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

// ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (password == null || password.Length < MinRequiredPasswordLength || password.Length > 200)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            var passwordHasher = IocContainer.Instance.Resolve<IPasswordHasher>();
            string passwordHash = passwordHasher.EncryptPassword(password);

            ILocaleProvider localeProvider = IocContainer.Instance.Resolve<ILocaleProvider>();
            MembershipData data = providerUserKey as MembershipData;
            LocaleData locale = data != null ? localeProvider.GetCultureByName(data.Locale) : localeProvider.DefaultCulture;

            user = new User
                {
                    Email = trimmedEmail, 
                    Name = trimmedUsername,
                    Password = passwordHash,
                    Locale = locale.ShortName
                };

            TransactionScope transactionScope = null;
            try
            {
                transactionScope = new TransactionScope();
                userService.Insert(user);
                userService.CreateRoleForUser(user, MonsterRoleProvider.RoleUser);
                transactionScope.Complete();

            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(CultureInfo.InvariantCulture, "Error creating user with email {0}", ex, email);
                status = MembershipCreateStatus.ProviderError;
                return null;
            }
            finally
            {
                if (transactionScope != null)
                    transactionScope.Dispose();
            }
            status = MembershipCreateStatus.Success;
            return null;
        }

        public override bool ValidateUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            var userRepo = IocContainer.Instance.Resolve<IUserRepository>();

            var user = userRepo.GetUserByEmail(email);
            if (user == null)
                return false;

            string salt;
            var passwordHasher = IocContainer.Instance.Resolve<IPasswordHasher>();
            try
            {                
                salt = passwordHasher.GetSaltFromHash(user.Password);
            }
            catch(MonsterSecurityException ex)
            {
                Logger.ErrorFormat(CultureInfo.InvariantCulture, "User with email {0} has invalid password hash", ex, user.Email);
                return false;
            }

            string hash = passwordHasher.EncryptPassword(password, salt);

            return user.Password.Equals(hash, StringComparison.InvariantCulture);
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser membershipUser)
        {
            throw new NotImplementedException();        
        }
    }
}