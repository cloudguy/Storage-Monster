using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using CloudBin.Core;
using CloudBin.Core.Domain;
using CloudBin.Core.Utilities;
using CloudBin.Data;
using CloudBin.Web.Core.Configuration;
using Common.Logging;

namespace CloudBin.Web.Core.Security
{
    public sealed class CookieAuthenticationService : IAuthenticationService
    {
        private readonly IUserSessionRepository _userSessionRepository;
        private readonly IAuthenticationConfiguration _authenticationConfiguration;
        private static readonly object Locker = new object();
        private static string _privateAuthCookieName;
        private volatile Func<HttpContext, bool>[] _staticContentCheckerInternal;
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public CookieAuthenticationService(IUserSessionRepository userSessionRepository, IAuthenticationConfiguration authenticationConfiguration)
        {
            _userSessionRepository = userSessionRepository;
            _authenticationConfiguration = authenticationConfiguration;
        }

        void IAuthenticationService.SignIn(User user)
        {
            throw new NotImplementedException();
        }

        void IAuthenticationService.SignOut()
        {
            ((IAuthenticationService)this).SignOut(HttpContext.Current);
        }

        void IAuthenticationService.SignOut(HttpContext httpContext)
        {
            ExpireAuthCookie(httpContext);
            var session = RequestContext.Current.GetUserSession();
            if (session != null)
            {
                session.Expires = DateTimeOffset.UtcNow;
                _userSessionRepository.Update(session);
            }
        }

        void IAuthenticationService.SlideExpire(HttpContext httpContext, UserSession session)
        {
            Verify.NotNull(()=>session);

            if (!_authenticationConfiguration.SlideExpire)
            {
                return;
            }

            var httpCookie = httpContext.Request.Cookies[AuthCookieName];
            if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
            {
                return;
            }

            DateTime expiration = session.IsPersistent ? DateTime.UtcNow.AddYears(1) : DateTime.UtcNow.Add(_authenticationConfiguration.SessionTimeout);
            session.Expires = expiration;
            _userSessionRepository.Update(session);
            httpCookie.Expires = expiration;
            httpContext.Response.Cookies.Add(httpCookie);
        }

        void IAuthenticationService.AuthenticateRequest(HttpContext httpContext)
        {
            var request = httpContext.Request;

            if (_authenticationConfiguration.DoNotAuthenticateScriptAndContent && StaticContentCheckers.Any(checker => checker(httpContext)))
            {
                return;
            }

            var authCookieName = AuthCookieName;
            var httpCookie = request.Cookies[authCookieName];
            if (httpCookie == null)
            {
                SetUser(httpContext, null);
                return;
            }

            CookieProtector protector = null;
            try
            {
                protector = new CookieProtector(_authenticationConfiguration);
                AuthenticationCookie authenticationCookie;
                if (!ValidateAuthCookie(protector, httpCookie, out authenticationCookie))
                {
                    SetUser(httpContext, null);
                    return;
                }
                var session = _userSessionRepository.GetSessionByToken(authenticationCookie.SessionId, true);
                if (session == null || session.Expires <= DateTime.UtcNow)
                {
                    SetUser(httpContext, null);
                    return;
                }
                ((IAuthenticationService) this).SlideExpire(httpContext, session);
                SetUser(httpContext, session.User);
                RequestContext.Current.SetUserSession(session);
            }
            catch
            {
                SetUser(httpContext, null);
                throw;
            }
            finally
            {
                if (protector != null)
                {
                    ((IDisposable) protector).Dispose();
                }
            }
        }


        public string AuthCookieName
        {
            get
            {
                if (_privateAuthCookieName == null)
                {
                    lock (Locker)
                    {
                        if (_privateAuthCookieName == null)
                            _privateAuthCookieName = CookieHelper.GetCookieName(_authenticationConfiguration.CookieName);
                    }
                }
                return _privateAuthCookieName;
            }
        }
        
        private IEnumerable<Func<HttpContext, bool>> StaticContentCheckers
        {
            get
            {
                if (_staticContentCheckerInternal == null)
                {
                    lock (Locker)
                    {
                        if (_staticContentCheckerInternal == null)
                        {
                            _staticContentCheckerInternal = RequestCheckersFactory.CreateStaticContentCheckers(HttpContext.Current);
                        }
                    }
                }
                return _staticContentCheckerInternal;
            }
        }

        private void ExpireAuthCookie(HttpContext httpContext)
        {
            var httpCookie = httpContext.Request.Cookies[AuthCookieName];
            if (httpCookie != null)
            {
                CookieHelper.ExpireCookie(httpCookie, httpContext);
            }
        }

        private void SetCookie(CookieProtector protector, UserSession session, HttpResponse response)
        {
            AuthenticationCookie authenticationCookie = new AuthenticationCookie(session.Token);
            var cookieName = AuthCookieName;
            var httpCookie = new HttpCookie(cookieName, protector.Protect(authenticationCookie.Serialize()))
            {
                HttpOnly = true,
                Secure = _authenticationConfiguration.RequireSSL,
                Expires = session.Expires.DateTime
            };
            response.Cookies.Add(httpCookie);
        }

        private bool ValidateAuthCookie(CookieProtector protector, HttpCookie httpCookie, out AuthenticationCookie authenticationCookie)
        {
            authenticationCookie = null;
            try
            {
                byte[] data;
                if (!protector.Validate(httpCookie.Value, out data))
                {
                    return false;
                }
                authenticationCookie = AuthenticationCookie.Deserialize(data);
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("Auth cookie validation error", ex);
                return false;
            }
        }

        private void SetUser(HttpContext context, User user)
        {
            bool isAutheticated = user != null;
            SetUser(context, user, isAutheticated);
        }

        private void SetUser(HttpContext context, User user, bool isAutheticated)
        {
            if (!isAutheticated)
            {
                ExpireAuthCookie(context);
            }
            Identity identity = new Identity(user, isAutheticated);
            GenericPrincipal principal = new GenericPrincipal(identity, null);
            context.User = principal;
            RequestContext.Current.SetUser(user);
        }
    }
}
