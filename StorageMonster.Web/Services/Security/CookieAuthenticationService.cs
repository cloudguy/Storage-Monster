using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Services.Security;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;


namespace StorageMonster.Web.Services.Security
{
    public class CookieAuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        private readonly AuthConfigurationSection _configuration;

		public CookieAuthenticationService(IUserService userService, ISessionService sessionService)
        {
			_userService = userService;
			_sessionService = sessionService;
            _configuration = (AuthConfigurationSection)ConfigurationManager.GetSection(AuthConfigurationSection.SectionLocation);
        }
        public void SignIn(User user, bool createPersistentCookie)
        {
            if (!_configuration.CookieAuth.AllowMultipleLogons)
                _sessionService.ClearUserSessions(user.Id);

            string sessionToken = Guid.NewGuid().ToString("N",CultureInfo.InvariantCulture);
            DateTime expiration;
            var cookie = CreateAuthCookie(sessionToken, createPersistentCookie, out expiration);
            Session session = new Session
            {
                User = user,
                Token = sessionToken,
                Expires = expiration
            };
            _sessionService.Insert(session);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

		public void SignOut()
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[_configuration.CookieAuth.AuthenticationCookieName];
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                _sessionService.ExpireSession(authCookie.Value);

            HttpCookie expiredCookie = new HttpCookie(_configuration.CookieAuth.AuthenticationCookieName)
                {
                    Expires = DateTime.UtcNow.AddDays(-1d)
                };
			HttpContext.Current.Response.Cookies.Add(expiredCookie);
        }

        private HttpCookie CreateAuthCookie(string userData, bool persistent, out DateTime expiration)
        {
            var cookie = new HttpCookie(_configuration.CookieAuth.AuthenticationCookieName, userData);
            expiration = DateTime.UtcNow.AddMinutes(_configuration.CookieAuth.AuthenticationExpiration);
            if (persistent)
                cookie.Expires = expiration;

            return cookie;
        }

        public void SlideExpire(HttpContext httpContext)
        {
            if (!_configuration.CookieAuth.AuthenticationSlidingExpiration)
                return;

            //updating session expiration in db

            HttpCookie authCookie = httpContext.Request.Cookies[_configuration.CookieAuth.AuthenticationCookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value)) return;

            DateTime cookieTime = authCookie.Expires.ToUniversalTime();

            var session = _sessionService.GetSessionByToken(authCookie.Value, true);

            if (session == null)
                return;

            DateTimeOffset expiration = DateTimeOffset.UtcNow.AddMinutes(_configuration.CookieAuth.AuthenticationExpiration);
#warning what was that?
           // if (session.Expiration == null || !session.Expiration.Value.Equals(cookieTime))
           _sessionService.UpdateSessionExpiration(session.Token, expiration);

            if (authCookie.Expires > DateTime.UtcNow) //not persistent cookie
            {
                authCookie.Expires = expiration.DateTime;
                httpContext.Response.Cookies.Add(authCookie);
            }
        }

        public void AuthorizeRequest()
        {
            try
            {
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[_configuration.CookieAuth.AuthenticationCookieName];
                if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
                {
                    SetUser(HttpContext.Current, null, UserRole.None, false);
                    return;
                }
                
                var session = _sessionService.GetSessionByToken(authCookie.Value, true);
                if (session == null || session.Expires <= DateTimeOffset.UtcNow)
                {
                    SetUser(HttpContext.Current, null, UserRole.None, false);
                    return;
                }
                SetUser(HttpContext.Current, session.User, session.User.UserRole, true);
            }
            catch
            {
                SetUser(HttpContext.Current, null, UserRole.None, false);
                throw;
            }
        }

        private static void SetUser(HttpContext context, User user, UserRole role, bool isAutheticated)
        {
            Identity identity = new Identity(user, isAutheticated);
            string roleAsString = role.ToString();
            Principal principal = new Principal(identity, new[] { roleAsString });
            context.User = principal;
        }
    }
}
