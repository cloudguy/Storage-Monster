using CloudBin.Core.Domain;
using System.Web;

namespace CloudBin.Web.Utilities.Security
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
