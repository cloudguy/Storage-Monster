using System.Collections.Generic;
using System.Resources;

namespace StorageMonster.Services
{
    public interface ITimeZonesProvider
    {
        IEnumerable<TimeZoneData> GetTimezones();
        void Init();
        TimeZoneData Gmt { get; }
        TimeZoneData GetTimeZoneByIdOrDefault(int id);
    }
}
