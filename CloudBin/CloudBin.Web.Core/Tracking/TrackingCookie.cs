using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace CloudBin.Web.Core.Tracking
{
    internal sealed class TrackingCookie
    {
        private readonly IDictionary<string, string> _trackingData;

        public IDictionary<string, string> TrackingData { get { return _trackingData; } }

        public TrackingCookie(IDictionary<string, string> trackingData)
        {
            _trackingData = trackingData;
        }

        private TrackingCookie(byte[] data)
        {
            _trackingData = JsonConvert.DeserializeObject<IDictionary<string, string>>(Encoding.UTF8.GetString(data));
        }

        public static TrackingCookie Deserialize(byte[] data)
        {
            return new TrackingCookie(data);
        }

        public byte[] Serialize()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_trackingData, Formatting.None));
        }
    }
}
