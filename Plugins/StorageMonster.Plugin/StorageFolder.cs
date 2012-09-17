using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Plugin
{
    [Serializable]
    public class StorageFolder : StorageItem
    {
        public override StorageItemType Itemtype { get { return StorageItemType.Folder; } }
    }
}
