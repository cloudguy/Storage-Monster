using System.Web;
using StorageMonster.Domain;

namespace StorageMonster.Web.Services.Security
{
    public interface IAuthenticationService
    {
        void SignIn(User user, bool createPersistentCookie);
        void SignOut();
        void SlideExpire(HttpContext httpContext);
        void AuthorizeRequest();
    }
}
