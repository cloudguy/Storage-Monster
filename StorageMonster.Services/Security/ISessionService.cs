using System;
using StorageMonster.Domain;

namespace StorageMonster.Services.Security
{
	public interface ISessionService
	{
        Session GetSessionByToken(string token, bool fetchUser);
		Session Insert(Session session);
		void ExpireSession(string sessionToken);
		void UpdateSessionExpiration(string sessionToken, DateTimeOffset expiration);
        void ClearUserSessions(int userId);
	}
}
