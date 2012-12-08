namespace StorageMonster.Services
{
    public interface ISweeper
    {
        void CleanUpExpiredSessions();
        void CleanUpExpiredResetPasswordsRequests();
        void CleanUp();
        void CleanUp(bool closeConnection);
    }
}
