using System.Web.Security;

namespace StorageMonster.Services.Security
{
    public interface IMembershipService
    {
        int MinPasswordLength { get; }
        bool ValidateUser(string email, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string userEmail, string locale);
    }
}
