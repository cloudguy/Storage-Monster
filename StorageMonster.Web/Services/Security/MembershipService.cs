using System;
using System.Web.Security;
using StorageMonster.Domain;
using System.Globalization;
using StorageMonster.Common;
using StorageMonster.Services;

namespace StorageMonster.Web.Services.Security
{
    public class MembershipService : IMembershipService
    {
        protected IUserService UserService { get; set; }
        public MembershipService(IUserService userService)
        {
            UserService = userService;
            if (Membership.Provider is MonsterMembershipProvider)
                Provider = Membership.Provider;           
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
    }
}
