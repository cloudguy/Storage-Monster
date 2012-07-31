using System;
using StorageMonster.Domain;

namespace StorageMonster.Services.Security
{
	public interface ISessionService
	{
		Session GetSessionByToken(string token);
		Session CreateSession(Session session);
		void ExpireSession(string sessionToken);
		void UpdateSessionExpiration(string sessionToken, DateTime expiration);
        void ClearUserSessions(int userId);
	}
}
