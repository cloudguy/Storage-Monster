using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Common;
using System.Runtime.Serialization;

namespace StorageMonster.Plugin
{
    [Serializable]
    public class PluginException : MonsterException
    {
        public PluginErrorCodes ErrorCode { get; protected set; }

        public PluginException(PluginErrorCodes errorCode)
            : base()
        {
            ErrorCode = errorCode;
        }

        public PluginException(PluginErrorCodes errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public PluginException(PluginErrorCodes errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        protected PluginException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}