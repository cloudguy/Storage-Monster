using StorageMonster.DB.Domain;

namespace StorageMonster.DB.Repositories
{
    public interface ISessionRepository
    {
        Session CreateSession(Session session);
        Session GetSessionByToken(string token);
    }
}
