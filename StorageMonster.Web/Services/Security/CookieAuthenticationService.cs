using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Services.Security;
using System;
using System.Configuration;
using System.Globalization;
using System.Web;


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
            string cookieName = CookieHelper.GetCookieName(_configuration.CookieAuth.AuthenticationCookieName, HttpContext.Current.Request.ApplicationPath);
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                _sessionService.ExpireSession(authCookie.Value);

            HttpCookie expiredCookie = new HttpCookie(cookieName)
                {
                    Expires = DateTime.UtcNow.AddDays(-1d)
                };
			HttpContext.Current.Response.Cookies.Add(expiredCookie);
        }

        private HttpCookie CreateAuthCookie(string userData, bool persistent, out DateTime expiration)
        {
            string cookieName = CookieHelper.GetCookieName(_configuration.CookieAuth.AuthenticationCookieName, HttpContext.Current.Request.ApplicationPath);
            var cookie = new HttpCookie(cookieName, userData);
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
            string cookieName = CookieHelper.GetCookieName(_configuration.CookieAuth.AuthenticationCookieName, HttpContext.Current.Request.ApplicationPath);
            HttpCookie authCookie = httpContext.Request.Cookies[cookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value)) return;

            DateTime cookieTime = authCookie.Expires.ToUniversalTime();

            var session = _sessionService.GetSessionByToken(authCookie.Value, true);

            if (session == null)
                return;

            DateTimeOffset expiration = DateTimeOffset.UtcNow.AddMinutes(_configuration.CookieAuth.AuthenticationExpiration);
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
                string cookieName = CookieHelper.GetCookieName(_configuration.CookieAuth.AuthenticationCookieName, HttpContext.Current.Request.ApplicationPath);
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];
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
