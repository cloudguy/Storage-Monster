using System.Collections.Generic;
using System.Resources;

namespace StorageMonster.Services
{
    public interface ITimeZonesProvider
    {
        IEnumerable<TimeZoneData> GetTimezones();
        void Init(ResourceManager timeZonesResourceManager);
        TimeZoneData Gmt { get; }
        TimeZoneData GetTimeZoneByIdOrDefault(int id);
    }
}
