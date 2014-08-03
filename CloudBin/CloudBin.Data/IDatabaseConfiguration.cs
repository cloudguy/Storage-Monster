namespace CloudBin.Data
{
    public interface IDatabaseConfiguration
    {
        bool UseOptimisticLockForUsers { get; }
        bool UseOptimisticLockForStorageAccounts { get; }
    }
}
