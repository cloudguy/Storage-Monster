using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Utilities.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T value) where T : class;
        T Deserialize<T>(string value) where T : class;
    }
}
