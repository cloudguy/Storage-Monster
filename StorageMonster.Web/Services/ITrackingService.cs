using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace StorageMonster.Web.Services
{
    public interface ITrackingService
    {
        void SetLocaleTracking(HttpContext context);
        string GetTrackedLocaleName(HttpContext context);
    }
}
