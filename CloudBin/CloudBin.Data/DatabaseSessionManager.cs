namespace CloudBin.Data
{
    public static class DatabaseSessionManager
    {
        public static IDatabaseSessionManager Current { get; private set; }

        public static void SetDatabaseSessionManager(IDatabaseSessionManager dbSessionManager)
        {
            Current = dbSessionManager;
        }
    }
}
