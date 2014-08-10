namespace CloudBin.Scheduling.Core
{
    public interface IJob
    {
        string JobType { get; }
        string JobName { get; set; }
        void Execute(IJobParameters parameters);
    }
}
