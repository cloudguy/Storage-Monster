using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace StorageMonster.Plugin
{    
    [Serializable]
    public class StoragePathItem
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public StoragePathItem ChildItem { get; set; }

        public void AppendItem(StoragePathItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (ChildItem != null)
                ChildItem.AppendItem(item);
            else
                ChildItem = item;
        }
    }
}
