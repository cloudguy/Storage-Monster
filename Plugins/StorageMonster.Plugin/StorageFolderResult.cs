using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Plugin
{
    public class StorageFolderResult : StorageQueryResult
    {
        private readonly IList<StorageItem> _storageItems = new List<StorageItem>();

        public IEnumerable<StorageItem> StorageItems { get { return _storageItems; } }

        public StoragePathItem CurrentPath { get; set; }

        public string CurrentFolderName { get; set; }
        public string CurrentFolderUrl { get; set; }

        public void AddItem(StorageItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _storageItems.Add(item);
        }
    }
}
