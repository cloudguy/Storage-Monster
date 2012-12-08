using System.Web;

namespace StorageMonster.Web.Services.Security
{
    public interface IAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
        void SlideExpire(HttpContext httpContext);
        void AuthorizeRequest();
    }
}
