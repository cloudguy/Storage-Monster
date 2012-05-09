using System;
using System.Web.Security;

namespace StorageMonster.Services.Security
{
    public class MembershipService : IMembershipService
    {
        protected readonly MembershipProvider Provider;

        public MembershipService()
        {
            if (Membership.Provider is MonsterMembershipProvider)
                Provider = Membership.Provider;
            else
                throw new InvalidOperationException("Membership provider not supported");
        }

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
    }
}
