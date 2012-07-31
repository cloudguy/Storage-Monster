using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Plugin
{
    public class StorageQueryResult
    {
        private IList<StorageItem> _storageItems = new List<StorageItem>();
        private IList<string> _errors = new List<string>();

        public bool IsValid { get { return _errors != null && _errors.Count > 0; } }

        public IEnumerable<string> Errors { get { return _errors; } }

        public IEnumerable<StorageItem> StorageItems { get { return _storageItems; } }        

        public void AddError(string error)
        {
            if (string.IsNullOrEmpty(error))
                throw new ArgumentNullException("error");

            _errors.Add(error);
        }

        public void AddItem(StorageItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _storageItems.Add(item);
        }
    }
}
