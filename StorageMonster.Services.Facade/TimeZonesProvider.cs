using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace StorageMonster.Services.Facade
{
    public class TimeZonesProvider : ITimeZonesProvider
    {
        private ResourceManager _timeZonesResourceManager;
        private IList<TimeZoneData> _timezones;
        private TimeZoneData _gmt;

        public IEnumerable<TimeZoneData> GetTimezones()
        {
            if (_timeZonesResourceManager == null || _timezones == null)
                throw new InvalidOperationException("Time zones provider not initialized");

            return _timezones;
        }

        public TimeZoneData Gmt 
        { 
            get
            {
                if (_gmt == null)
                   throw new InvalidOperationException("Time zones provider not initialized");
                return _gmt;
            }
        }

        public TimeZoneData GetTimeZoneByIdOrDefault(int id)
        {
            if (_timezones == null || _gmt == null)
                throw new InvalidOperationException("Time zones provider not initialized");

            TimeZoneData timeZoneData = _timezones.Where(t => t.Id == id).FirstOrDefault();
            return timeZoneData ?? _gmt;
        }

        public void Init(ResourceManager timeZonesResourceManager)
        {
            if (timeZonesResourceManager == null)
                throw new ArgumentNullException("timeZonesResourceManager");

            _timeZonesResourceManager = timeZonesResourceManager;
            _timezones = new List<TimeZoneData>();

            _timezones.Add(new TimeZoneData(-43200, new TimeSpan(0, -12, 0, 0), "tz_m_12", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(-39600, new TimeSpan(0, -11, 0, 0), "tz_m_11", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(-36000, new TimeSpan(0, -10, 0, 0), "tz_m_10", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(-32400, new TimeSpan(0, -9, 0, 0), "tz_m_9", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(-28800, new TimeSpan(0, -8, 0, 0), "tz_m_8", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(-25200, new TimeSpan(0, -7, 0, 0), "tz_m_7", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(-21600, new TimeSpan(0, -6, 0, 0), "tz_m_6", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(-18000, new TimeSpan(0, -5, 0, 0), "tz_m_5", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(-14400, new TimeSpan(0, -4, 0, 0), "tz_m_4", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(-12600, new TimeSpan(0, -3, -30, 0), "tz_m_3_30", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(-10800, new TimeSpan(0, -3, 0, 0), "tz_m_3", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(-7200, new TimeSpan(0, -2, 0, 0), "tz_m_2", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(-3600, new TimeSpan(0, -1, 0, 0), "tz_m_1", _timeZonesResourceManager));

            _gmt = new TimeZoneData(0, new TimeSpan(0, 0, 0, 0), "tz_0", _timeZonesResourceManager);
            _timezones.Add(_gmt);

            _timezones.Add(new TimeZoneData(3600, new TimeSpan(0, 1, 0, 0), "tz_p_1", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(7200, new TimeSpan(0, 2, 0, 0), "tz_p_2", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(10800, new TimeSpan(0, 3, 0, 0), "tz_p_3", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(12600, new TimeSpan(0, 3, 30, 0), "tz_p_3_30", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(14400, new TimeSpan(0, 4, 0, 0), "tz_p_4", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(16200, new TimeSpan(0, 4, 30, 0), "tz_p_4_30", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(18000, new TimeSpan(0, 5, 0, 0), "tz_p_5", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(19800, new TimeSpan(0, 5, 30, 0), "tz_p_5_30", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(20700, new TimeSpan(0, 5, 45, 0), "tz_p_5_45", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(21600, new TimeSpan(0, 6, 0, 0), "tz_p_6", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(25200, new TimeSpan(0, 7, 0, 0), "tz_p_7", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(28800, new TimeSpan(0, 8, 0, 0), "tz_p_8", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(32400, new TimeSpan(0, 9, 0, 0), "tz_p_9", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(34200, new TimeSpan(0, 9, 30, 0), "tz_p_9_30", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(36000, new TimeSpan(0, 10, 0, 0), "tz_p_10", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(39600, new TimeSpan(0, 11, 0, 0), "tz_p_11", _timeZonesResourceManager));
            _timezones.Add(new TimeZoneData(43200, new TimeSpan(0, 12, 0, 0), "tz_p_12", _timeZonesResourceManager));
        }
    }
}
