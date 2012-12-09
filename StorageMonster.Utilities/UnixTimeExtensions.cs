using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Utilities
{
    public static class UnixTimeExtensions
    {
        public static DateTimeOffset ConvertFromUnixTimestamp(this Int64 timestamp)
        {
            DateTimeOffset origin = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);
            return origin.AddSeconds(timestamp);
        }

        public static Int64 ConvertToUnixTimestamp(this DateTimeOffset date)
        {
            DateTimeOffset origin = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);
            TimeSpan diff = date - origin;
            return Convert.ToInt64(diff.TotalSeconds);
        }
    }
}
