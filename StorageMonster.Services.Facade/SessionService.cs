using System;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;
using StorageMonster.Services.Security;

namespace StorageMonster.Services.Facade
{
	public class SessionService : ISessionService
	{
        protected ISessionRepository SessionRepository { get; set; }

		public SessionService(ISessionRepository sessionRepository)
		{
			SessionRepository = sessionRepository;
		}

        public Session Insert(Session session)
        {
            return SessionRepository.Insert(session);
        }
        
        public void ClearUserSessions(int userId)
        {
            SessionRepository.ClearUserSessions(userId);
        }

        public Session GetSessionByToken(string token, bool fetchUser)
        {
            return SessionRepository.GetSessionByToken(token, fetchUser);
        }

        public void ExpireSession(string sessionToken)
        {
            SessionRepository.UpdateExpiration(sessionToken, new DateTimeOffset(1900, 1, 1, 0, 0, 0, 0, TimeSpan.Zero));
        }

        public void UpdateSessionExpiration(string sessionToken, DateTimeOffset expiration)
        {
            SessionRepository.UpdateExpiration(sessionToken, expiration);
        }
    }
}
