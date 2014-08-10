using System.Web;
using CloudBin.Core.Domain;

namespace CloudBin.Web.Core.Security
{
    public interface IAuthenticationService
    {
        void SignIn(User user);
        void SignOut();
        void SignOut(HttpContext httpContext);
        void SlideExpire(HttpContext httpContext, UserSession session);
        void AuthenticateRequest(HttpContext httpContext);
        string AuthCookieName { get; }
    }
}
