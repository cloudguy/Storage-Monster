using StorageMonster.Database.Repositories;

namespace StorageMonster.Services.Facade
{
    public class Sweeper : ISweeper
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IResetPasswordRequestsRepository _resetPasswordRequestsRepository;

        public Sweeper(ISessionRepository sessionRepository, IResetPasswordRequestsRepository resetPasswordRequestsRepository)
        {
            _sessionRepository = sessionRepository;
            _resetPasswordRequestsRepository = resetPasswordRequestsRepository;
        }

        public void CleanUpExpiredSessions()
        {
            _sessionRepository.ClearExpiredSessions();
        }

        public void CleanUpExpiredResetPasswordsRequests()
        {
            _resetPasswordRequestsRepository.DeleteExpiredRequests();
        }

        public void CleanUp()
        {
            CleanUpExpiredSessions();
            CleanUpExpiredResetPasswordsRequests();
        }
    }
}
