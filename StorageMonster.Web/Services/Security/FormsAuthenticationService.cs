using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Services.Security;
using StorageMonster.Web.Services.Configuration;


namespace StorageMonster.Web.Services.Security
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        private readonly IWebConfiguration _webConfiguration;

		public FormsAuthenticationService(IUserService userService, ISessionService sessionService, IWebConfiguration webConfiguration)
        {
			_userService = userService;
			_sessionService = sessionService;
			_webConfiguration = webConfiguration;
        }
        public void SignIn(string email, bool createPersistentCookie)
        {
            string sessionToken = Guid.NewGuid().ToString("N",CultureInfo.InvariantCulture);
            string sessionAntiforgeryToken = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

			User user = _userService.GetUserByEmail(email);
            if (user ==null)
                throw new MonsterSecurityException(String.Format(CultureInfo.InvariantCulture, "User {0} not found", email));

            Session session = new Session
                {
                    UserId = user.Id,
                    Token = sessionToken                    
                };

            DateTime expiration;
            var cookie = CreateAuthCookie(email, sessionToken, createPersistentCookie, out expiration);

            session.Expiration = expiration;

            if (!_webConfiguration.AllowMultipleLogons)
                _sessionService.ClearUserSessions(user.Id);

			_sessionService.CreateSession(session);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

		public void SignOut()
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[_webConfiguration.AuthenticationCookieName];
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                _sessionService.ExpireSession(authCookie.Value);

			HttpCookie expiredCookie = new HttpCookie(FormsAuthentication.FormsCookieName)
                {
                    Expires = DateTime.UtcNow.AddDays(-1d)
                };
			HttpContext.Current.Response.Cookies.Add(expiredCookie);
        }

        private HttpCookie CreateAuthCookie(string email, string userData, bool persistent, out DateTime expiration)
        {
            var cookie = new HttpCookie(_webConfiguration.AuthenticationCookieName, userData);
            expiration = DateTime.UtcNow.AddMinutes(_webConfiguration.AuthenticationExpiration.TotalMinutes);
            if (persistent)
                cookie.Expires = expiration;

            return cookie;
        }

        public void SlideExpire(HttpContext httpContext)
        {
            if (!_webConfiguration.AuthenticationSlidingExpiration)
                return;

            //updating session expiration in db

            HttpCookie authCookie = httpContext.Request.Cookies[_webConfiguration.AuthenticationCookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value)) return;

            DateTime cookieTime = authCookie.Expires.ToUniversalTime();

            var session = _sessionService.GetSessionByToken(authCookie.Value);

            if (session == null)
                return;

            DateTime expiration = DateTime.UtcNow.Add(_webConfiguration.AuthenticationExpiration);
            if (session.Expiration == null || !session.Expiration.Value.Equals(cookieTime))
                _sessionService.UpdateSessionExpiration(session.Token, expiration);

            if (authCookie.Expires > DateTime.UtcNow) //not persistent cookie
            {
                authCookie.Expires = expiration;
                httpContext.Response.Cookies.Add(authCookie);
            }
        }

        public void AuthorizeRequest()
        {
            try
            {
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[_webConfiguration.AuthenticationCookieName];
                if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
                {
                    SetUser(HttpContext.Current, null, null, false);
                    return;
                }
                
                var session = _sessionService.GetSessionByToken(authCookie.Value);
                if (session == null || (session.Expiration != null && session.Expiration <= DateTime.UtcNow))
                {
                    SetUser(HttpContext.Current, null, null, false);
                    return;
                }

                User user = _userService.GetUserBySessionToken(session);

                string[] roles = _userService.GetRolesForUser(user).Select(r => r.Role).ToArray();
                SetUser(HttpContext.Current, user, roles, true);
            }
            catch
            {
                SetUser(HttpContext.Current, null, null, false);
                throw;
            }
        }

        private static void SetUser(HttpContext context, User user, string[] roles, bool isAutheticated)
        {
            Identity identity = new Identity(user, isAutheticated);
            Principal principal = new Principal(identity, roles);
            context.User = principal;
        }
    }
}
