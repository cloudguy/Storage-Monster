using StorageMonster.Database.Repositories;

namespace StorageMonster.Services.Facade
{
    public class Sweeper : ISweeper
    {
        protected ISessionRepository SessionRepository { get; set; }
        protected IResetPasswdRequestsRepository ResetPasswdRequestsRepository { get; set; }

        public Sweeper(ISessionRepository sessionRepository, IResetPasswdRequestsRepository resetPasswdRequestsRepository)
        {
            SessionRepository = sessionRepository;
            ResetPasswdRequestsRepository = resetPasswdRequestsRepository;
        }

        public void CleanUpExpiredSessions()
        {
            SessionRepository.ClearExpiredSessions();
        }

        public void CleanUpExpiredResetPasswordsRequests()
        {
            ResetPasswdRequestsRepository.DeleteExpiredRequests();
        }

        public void CleanUp()
        {
            CleanUpExpiredSessions();
            CleanUpExpiredResetPasswordsRequests();
        }
    }
}
