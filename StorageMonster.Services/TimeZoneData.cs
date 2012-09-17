using System;
using System.Resources;

namespace StorageMonster.Services
{
    public class TimeZoneData
    {
        public TimeZoneData(int id, TimeSpan offset, string timeZoneResourceName, ResourceManager timeZoneResourceManager)
        {
            _timeZoneResourceName = timeZoneResourceName;
            _timeZoneResourceManager = timeZoneResourceManager;
            _offset = offset;
            _id = id;
        }

        private readonly string _timeZoneResourceName;
        private readonly TimeSpan _offset;
        private readonly int _id;
        private readonly ResourceManager _timeZoneResourceManager;

        public string TimeZoneName
        {
            get { return _timeZoneResourceManager.GetString(_timeZoneResourceName); }
        }

        public TimeSpan Offset
        {
            get { return _offset; }
        }

        public int Id
        {
            get { return _id; }
        }
    }
}
