using StorageMonster.Domain;

namespace StorageMonster.Database.Repositories
{
    public interface ISessionRepository
    {
        Session CreateSession(Session session);
        Session GetSessionByToken(string token);
		Session UpdateExpiration(Session session);
        void ClearUserSessions(int userId);
        void ClearExpiredSessions();
    }
}
