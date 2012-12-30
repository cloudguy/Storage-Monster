using System;
using System.Collections.Generic;
using StorageMonster.Services;
using StorageMonster.Web.Properties;

namespace StorageMonster.Web.Services
{
    public class TimeZonesProvider : TimeZonesProviderBase
    {
        public override void Init()
        {
            TimeZonesResourceManager = TimeZonesResources.ResourceManager;
            TimeZones = new List<TimeZoneData>();

            TimeZones.Add(new TimeZoneData(-43200, new TimeSpan(0, -12, 0, 0), "tz_m_12", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(-39600, new TimeSpan(0, -11, 0, 0), "tz_m_11", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(-36000, new TimeSpan(0, -10, 0, 0), "tz_m_10", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(-32400, new TimeSpan(0, -9, 0, 0), "tz_m_9", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(-28800, new TimeSpan(0, -8, 0, 0), "tz_m_8", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(-25200, new TimeSpan(0, -7, 0, 0), "tz_m_7", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(-21600, new TimeSpan(0, -6, 0, 0), "tz_m_6", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(-18000, new TimeSpan(0, -5, 0, 0), "tz_m_5", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(-14400, new TimeSpan(0, -4, 0, 0), "tz_m_4", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(-12600, new TimeSpan(0, -3, -30, 0), "tz_m_3_30", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(-10800, new TimeSpan(0, -3, 0, 0), "tz_m_3", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(-7200, new TimeSpan(0, -2, 0, 0), "tz_m_2", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(-3600, new TimeSpan(0, -1, 0, 0), "tz_m_1", TimeZonesResourceManager));

            GmtInternal = new TimeZoneData(0, new TimeSpan(0, 0, 0, 0), "tz_0", TimeZonesResourceManager);
            TimeZones.Add(GmtInternal);

            TimeZones.Add(new TimeZoneData(3600, new TimeSpan(0, 1, 0, 0), "tz_p_1", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(7200, new TimeSpan(0, 2, 0, 0), "tz_p_2", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(10800, new TimeSpan(0, 3, 0, 0), "tz_p_3", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(12600, new TimeSpan(0, 3, 30, 0), "tz_p_3_30", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(14400, new TimeSpan(0, 4, 0, 0), "tz_p_4", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(16200, new TimeSpan(0, 4, 30, 0), "tz_p_4_30", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(18000, new TimeSpan(0, 5, 0, 0), "tz_p_5", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(19800, new TimeSpan(0, 5, 30, 0), "tz_p_5_30", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(20700, new TimeSpan(0, 5, 45, 0), "tz_p_5_45", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(21600, new TimeSpan(0, 6, 0, 0), "tz_p_6", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(25200, new TimeSpan(0, 7, 0, 0), "tz_p_7", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(28800, new TimeSpan(0, 8, 0, 0), "tz_p_8", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(32400, new TimeSpan(0, 9, 0, 0), "tz_p_9", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(34200, new TimeSpan(0, 9, 30, 0), "tz_p_9_30", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(36000, new TimeSpan(0, 10, 0, 0), "tz_p_10", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(39600, new TimeSpan(0, 11, 0, 0), "tz_p_11", TimeZonesResourceManager));
            TimeZones.Add(new TimeZoneData(43200, new TimeSpan(0, 12, 0, 0), "tz_p_12", TimeZonesResourceManager));
        }
    }
}