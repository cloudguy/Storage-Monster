namespace StorageMonster.Services
{
    public interface ISweeper
    {
        void CleanUpExpiredSessions();
        void CleanUp();
    }
}
