using System;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;

namespace StorageMonster.Database.Nhibernate.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        public Session CreateSession(Session session)
        {
            throw new NotImplementedException();
        }

        public Session GetSessionByToken(string token, bool fetchUser)
        {
            var query = SessionManager.CurrentSession.QueryOver<Session>()
                          .Where(s => s.Token == token);
            if (fetchUser)
                query = query.Fetch(s => s.User).Eager;
            return query.SingleOrDefault();
        }

        public Session Update(Session session)
        {
            SessionManager.CurrentSession.Update(session);
            return session;
        }

        public void ClearUserSessions(int userId)
        {
            throw new NotImplementedException();
        }

        public void ClearExpiredSessions()
        {
            throw new NotImplementedException();
        }
    }
}
