using StorageMonster.Domain;

namespace StorageMonster.Database.Repositories
{
    public interface ISessionRepository
    {
        Session CreateSession(Session session);
        Session GetSessionByToken(string token, bool fetchUser);
		Session Update(Session session);
        void ClearUserSessions(int userId);
        void ClearExpiredSessions();
    }
}
