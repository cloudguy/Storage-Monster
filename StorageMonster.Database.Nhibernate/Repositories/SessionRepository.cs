using System;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;

namespace StorageMonster.Database.Nhibernate.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        public Session Insert(Session session)
        {
            SessionManager.CurrentSession.Save(session);
            return session;
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
            SessionManager.CurrentSession.Flush();
            return session;
        }

        public void ClearUserSessions(int userId)
        {
            SessionManager.CurrentSession.CreateQuery("delete Session s where s.User.Id = :userId")
                .SetParameter("userId", userId)
                .ExecuteUpdate();
            SessionManager.CurrentSession.Flush();
        }

        public void UpdateExpiration(string sessionToken, DateTimeOffset expiration)
        {
            SessionManager.CurrentSession.CreateQuery("update Session set Expires = :expires where Token = :token")
                .SetParameter("token", sessionToken)
                .SetParameter("expires", expiration)
                .ExecuteUpdate();
            SessionManager.CurrentSession.Flush();
        }

        public void ClearExpiredSessions()
        {
            throw new NotImplementedException();
        }
    }
}
