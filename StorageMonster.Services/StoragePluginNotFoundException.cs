using StorageMonster.Common;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace StorageMonster.Services
{
    [Serializable]
    public class StoragePluginNotFoundException : MonsterException
    {
        public StoragePluginNotFoundException()
        {
        }
        public StoragePluginNotFoundException(int id)
            : base (string.Format(CultureInfo.InvariantCulture, "Plugin [{0}] not found", id))             
        {
        }

        public StoragePluginNotFoundException(string message)
            : base(message)
        {
        }

        public StoragePluginNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected StoragePluginNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
