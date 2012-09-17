using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace StorageMonster.Utilities.Serialization
{
#warning remove?
    public class JsonSerializer : ISerializer
    {
        public virtual string Serialize<T>(T value) where T : class
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return JsonConvert.SerializeObject(value, Formatting.None);
        }

        public virtual T Deserialize<T>(string value) where T : class
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (JsonReaderException ex)
            {
                throw new SerializationException("Deserialization failed", ex);
            }
        }
    }
}
