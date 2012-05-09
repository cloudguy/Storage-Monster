using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using StorageMonster.DB.Domain;
using StorageMonster.DB.Repositories;

namespace StorageMonster.Services.Security
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        protected readonly IUserRepository UserRepository;
        protected readonly ISessionRepository ServiceRepository;

        public FormsAuthenticationService(IUserRepository userRepository, ISessionRepository serviceRepository)
        {
            UserRepository = userRepository;
            ServiceRepository = serviceRepository;
        }
        public void SignIn(string email, bool createPersistentCookie)
        {
            string sessionToken = Guid.NewGuid().ToString("N",CultureInfo.InvariantCulture);
            string sessionAntiforgeryToken = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

            User user = UserRepository.GetByEmail(email);
            if (user ==null)
                throw new StorageMonsterSecurityException(String.Format(CultureInfo.InvariantCulture, "User {0} not found", email));

            Session session = new Session
                {
                    UserId = user.Id,
                    SessionToken = sessionToken,
                    SessionAntiforgeryToken = sessionAntiforgeryToken
                };
            var cookie = CreateAuthCookie(email, sessionToken, createPersistentCookie);

            if (createPersistentCookie)
                session.Expiration = cookie.Expires.ToUniversalTime();

            ServiceRepository.CreateSession(session);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        protected static HttpCookie CreateAuthCookie(string email, string userData, bool persistent)
        {
            DateTime issued = DateTime.UtcNow;
            HttpCookie fooCookie = FormsAuthentication.GetAuthCookie(email, true);
            int formsTimeout = Convert.ToInt32((fooCookie.Expires - DateTime.UtcNow).TotalMinutes);

            DateTime expiration = DateTime.UtcNow.AddMinutes(formsTimeout);
            string cookiePath = FormsAuthentication.FormsCookiePath;

            var ticket = new FormsAuthenticationTicket(0, email, issued, expiration, true, userData, cookiePath);
            return CreateAuthCookie(ticket, expiration, persistent);
        }

        protected static HttpCookie CreateAuthCookie(FormsAuthenticationTicket ticket, DateTime expiration, bool persistent)
        {
            string creamyFilling = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, creamyFilling)
            {
                Domain = FormsAuthentication.CookieDomain,
                Path = FormsAuthentication.FormsCookiePath
            };
            if (persistent)
            {
                cookie.Expires = expiration;
            }

            return cookie;
        }
    }
}
