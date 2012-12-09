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

		public Session GetSessionByToken(string token)
		{
			return SessionRepository.GetSessionByToken(token);
		}

		public Session CreateSession(Session session)
		{
			return SessionRepository.CreateSession(session);			
		}

		public void ExpireSession(string sessionToken)
		{
			Session session = new Session
			{
				Token = sessionToken,
				Expiration = new DateTime(1900, 1, 1)
			};
			SessionRepository.Update(session);
		}

		public void UpdateSessionExpiration(string sessionToken, DateTime expiration)
		{
			Session session = new Session
			{
				Token = sessionToken,
				Expiration = expiration
			};
			SessionRepository.Update(session);
		}
        public void ClearUserSessions(int userId)
        {
            SessionRepository.ClearUserSessions(userId);
        }
	}
}
