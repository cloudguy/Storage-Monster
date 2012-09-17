using StorageMonster.Domain;

namespace StorageMonster.Database.Repositories
{
    public interface IResetPasswordRequestsRepository
    {
        ResetPasswordRequest GetActiveRequestByToken(string token);
        void DeleteRequest(int id);
        ResetPasswordRequest CreateRequest(ResetPasswordRequest request);
        void DeleteExpiredRequests();
    }
}
