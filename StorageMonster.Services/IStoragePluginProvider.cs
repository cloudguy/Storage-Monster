using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StorageMonster.Plugin;

namespace StorageMonster.Services
{
    public interface IStoragePluginProvider
    {
        IStoragePlugin GetPlugin(int storageId);
        void SetPlugin(int pluginId, IStoragePlugin storagePlugin);
    }
}
