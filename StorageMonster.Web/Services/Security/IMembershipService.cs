using System.Web.Security;
using System;
using StorageMonster.Domain;

namespace StorageMonster.Web.Services.Security
{
    public interface IMembershipService
    {
        int MinPasswordLength { get; }
        int MaxPasswordLength { get; }
        int MaxEmailLength { get; }
        int MaxUserNameLength { get; }
        bool ValidateUser(string email, string password);
        MembershipCreateStatus CreateUser(string email, string password, string userName, string locale, int timezone);
        User UpdateUser(int userId, string userName, string locale, int timezone, DateTime stamp, Identity identity);
        void RequestPasswordReset(string email, string siteUrl, Func<string,string> resetUrlGenerator);
        ResetPasswordRequest GetActivePasswordResetRequestByToken(string token);
        void ChangePassword(string resetToken, string newPassword);
        User ChangePassword(int userId, string newPassword, string oldPassword, DateTime userStamp);
    }
}
