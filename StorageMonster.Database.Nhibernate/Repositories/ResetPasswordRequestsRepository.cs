using System;
using StorageMonster.Database.Repositories;

namespace StorageMonster.Database.Nhibernate.Repositories
{
    public class ResetPasswordRequestsRepository : IResetPasswordRequestsRepository
    {
        public Domain.ResetPasswordRequest GetActiveRequestByToken(string token)
        {
            throw new NotImplementedException();
        }

        public void DeleteRequest(int id)
        {
            throw new NotImplementedException();
        }

        public Domain.ResetPasswordRequest CreateRequest(Domain.ResetPasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public void DeleteExpiredRequests()
        {
            throw new NotImplementedException();
        }
    }
}
