using StorageMonster.Database.Repositories;

namespace StorageMonster.Services.Facade
{
    public class Sweeper : ISweeper
    {
        protected ISessionRepository SessionRepository { get; set; }

        public Sweeper(ISessionRepository sessionRepository)
        {
            SessionRepository = sessionRepository;
        }

        public void CleanUpExpiredSessions()
        {
            SessionRepository.ClearExpiredSessions();
        }

        public void CleanUp()
        {
            CleanUpExpiredSessions();
        }
    }
}
