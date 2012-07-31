using System.Web;

namespace StorageMonster.Web.Services.Security
{
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
        void SlideExpire(HttpContext httpContext);
        void AuthorizeRequest();
    }
}
