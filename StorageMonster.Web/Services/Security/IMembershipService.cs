using System.Web.Security;
using System;
using StorageMonster.Domain;

namespace StorageMonster.Web.Services.Security
{
    public interface IMembershipService
    {
        int MinPasswordLength { get; }
        bool ValidateUser(string email, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string userEmail, string locale);
        User UpdateUser(int userId, string userName, string locale, DateTime stamp);
        void RequestPasswordReset(string email, string siteurl, Func<string,string> urlGenerator);
        ResetPasswordRequest GetActivePasswordResetRequestByToken(string token);
        void ChangePassword(string resetToken, string newPassword);
        User ChangePassword(int userId, string newPassword, string oldPassword, DateTime userStamp);
    }
}
