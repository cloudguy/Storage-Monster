namespace CloudBin.Web.Core.Tracking
{
    public interface ITrackingService
    {
        string GetTrackedValue(string key);
        bool TryGetTrackedValue(string key, out string value);
        void SetTrackedValue(string key, string value);
        string SerializeTrackedData();
        bool Dirty { get; }
    }
}
