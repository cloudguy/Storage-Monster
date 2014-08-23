namespace CloudBin.Web.Core.Tracking
{
    public interface ITrackingService
    {
        string GetTrackedValue(string key);
        void SetTrackedValue(string key, string value);
        string SerializeTrackedData();
        bool Dirty { get; }
    }
}
