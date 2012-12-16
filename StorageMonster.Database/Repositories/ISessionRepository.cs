using System;
using StorageMonster.Domain;

namespace StorageMonster.Database.Repositories
{
    public interface ISessionRepository
    {
        Session Insert(Session session);
        Session GetSessionByToken(string token, bool fetchUser);
		Session Update(Session session);
        void UpdateExpiration(string sessionToken, DateTimeOffset expiration);
        void ClearUserSessions(int userId);
        void ClearExpiredSessions();
    }
}
