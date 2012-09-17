using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace StorageMonster.Utilities.Serialization
{
    public class BinarySerializer : ISerializer
    {
        private readonly IFormatter _formatter = new BinaryFormatter();

        public string Serialize<T>(T value) where T : class
        {
            if (value == null)
                throw new ArgumentNullException("value");

            using (var stream = new MemoryStream())
            {
                _formatter.Serialize(stream, value);
                var bytes = stream.ToArray();
                return Convert.ToBase64String(bytes);
            }
        }

        public T Deserialize<T>(string value) where T : class
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            try
            {
                byte[] bytes = Convert.FromBase64String(value);
                using (var stream = new MemoryStream(bytes))
                {
                    return _formatter.Deserialize(stream) as T;
                }
            }
            catch (FormatException ex)
            {
                throw new SerializationException("Deserialization failed", ex);
            }
        }
    }
}
