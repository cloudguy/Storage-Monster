using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Domain;

namespace StorageMonster.Database.Repositories
{
    public interface IResetPasswdRequestsRepository
    {
        ResetPasswordRequest GetActiveRequestByToken(string token);
        void DeleteRequest(int id);
        ResetPasswordRequest CreateRequest(ResetPasswordRequest request);
        void DeleteExpiredRequests();
    }
}
