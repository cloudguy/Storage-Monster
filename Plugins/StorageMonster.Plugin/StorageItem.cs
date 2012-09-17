using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Plugin
{
    [Serializable]
    public abstract class StorageItem
    {
        public virtual string Name { get; set; }
        public abstract StorageItemType Itemtype { get; }
        public virtual string Path { get; set; }
        public virtual int StorageAccountId { get; set; }
    }
}
