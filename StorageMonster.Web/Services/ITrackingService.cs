using System.Web;

namespace StorageMonster.Web.Services
{
    public interface ITrackingService
    {
        void SetLocaleTracking(HttpContext context);
        string GetTrackedLocaleName(HttpContext context);
    }
}