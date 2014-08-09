using CloudBin.Core;
using CloudBin.Core.Domain;

namespace CloudBin.Web.Utilities
{
    public static class RequestContextProviderExtensions
    {
        private const string UserKey = "current_user";
        private const string UserSessionKey = "current_user_session";

        public static User GetUser(this IRequestContextProvider contextProvider)
        {
            return contextProvider.GetValue<User>(UserKey);
        }

        public static void SetUser(this IRequestContextProvider contextProvider, User user)
        {
            contextProvider.SetValue(UserKey, user);
        }

        public static UserSession GetUserSession(this IRequestContextProvider contextProvider)
        {
            return contextProvider.GetValue<UserSession>(UserSessionKey);
        }

        public static void SetUserSession(this IRequestContextProvider contextProvider, UserSession userSession)
        {
            contextProvider.SetValue(UserKey, UserSessionKey);
        }
    }
}
