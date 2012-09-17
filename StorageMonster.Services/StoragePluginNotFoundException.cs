using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using StorageMonster.Common;
using System.Globalization;

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
