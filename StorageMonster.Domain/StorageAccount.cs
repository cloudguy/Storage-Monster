using System;
using System.Collections.Generic;

namespace StorageMonster.Domain
{
    public class StorageAccount
    {
        public virtual int Id { get; set; }
        public virtual User User { get; set; }
        public virtual StoragePluginDescriptor StoragePlugin { get; set; }
        public virtual String AccountName { get; set; }
        public virtual DateTime Stamp { get; set; }
        public virtual long Version { get; set; }
        public virtual IList<StorageAccountSetting> Settings { get; set; }
    }
}
