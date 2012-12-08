using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace StorageMonster.Services
{
    public abstract class TimeZonesProviderBase : ITimeZonesProvider
    {
        protected ResourceManager TimeZonesResourceManager;
        protected IList<TimeZoneData> TimeZones;
        protected TimeZoneData GmtInternal;

        public IEnumerable<TimeZoneData> GetTimezones()
        {
            if (TimeZonesResourceManager == null || TimeZones == null)
                throw new InvalidOperationException("Time zones provider not initialized");

            return TimeZones;
        }

        public TimeZoneData Gmt 
        { 
            get
            {
                if (GmtInternal == null)
                   throw new InvalidOperationException("Time zones provider not initialized");
                return GmtInternal;
            }
        }

        public TimeZoneData GetTimeZoneByIdOrDefault(int id)
        {
            if (TimeZones == null || GmtInternal == null)
                throw new InvalidOperationException("Time zones provider not initialized");

            TimeZoneData timeZoneData = TimeZones.FirstOrDefault(t => t.Id == id);
            return timeZoneData ?? GmtInternal;
        }

        public abstract void Init();
    }
}
