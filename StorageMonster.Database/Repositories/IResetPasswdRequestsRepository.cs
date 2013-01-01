using StorageMonster.Domain;

namespace StorageMonster.Database.Repositories
{
    public interface IResetPasswordRequestsRepository
    {
        ResetPasswordRequest GetActiveRequestByToken(string token);
        void DeleteRequest(ResetPasswordRequest request);
        ResetPasswordRequest Insert(ResetPasswordRequest request);
        void DeleteExpiredRequests();
    }
}
