using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace StorageMonster.Utilities.Serialization
{
#warning remove?
    public class JsonBase64Serializer : JsonSerializer
    { 
        public override string Serialize<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException("value"); 
          
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(base.Serialize(value)));            
        }

        public override T Deserialize<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            try
            {
                byte[] bytes = Convert.FromBase64String(value);                
                return base.Deserialize<T>(Encoding.UTF8.GetString(bytes));                
            }
            catch (FormatException ex)
            {
                throw new SerializationException("Deserialization failed", ex);
            }
            catch (DecoderFallbackException ex)
            {
                throw new SerializationException("Deserialization failed", ex);
            }
        }
    }
}
