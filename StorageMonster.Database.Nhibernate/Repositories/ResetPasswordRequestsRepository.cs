using System;
using StorageMonster.Database.Repositories;
using StorageMonster.Domain;

namespace StorageMonster.Database.Nhibernate.Repositories
{
    public class ResetPasswordRequestsRepository : IResetPasswordRequestsRepository
    {
        public ResetPasswordRequest GetActiveRequestByToken(string token)
        {
            return SessionManager.CurrentSession.QueryOver<ResetPasswordRequest>()
                          .Where(r => r.Token == token)
                          .And(r=>r.Expires > DateTimeOffset.UtcNow)
                          .SingleOrDefault();
        }

        public void DeleteRequest(ResetPasswordRequest request)
        {
            SessionManager.CurrentSession.Delete(request);
        }

        public ResetPasswordRequest Insert(ResetPasswordRequest request)
        {
            SessionManager.CurrentSession.Save(request);
            SessionManager.CurrentSession.Flush();
            return request;
        }

        public void DeleteExpiredRequests()
        {
            throw new NotImplementedException();
        }
    }
}
