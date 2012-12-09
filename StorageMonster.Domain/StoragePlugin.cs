using System;

namespace StorageMonster.Domain
{
    public class StoragePlugin
    {
        public virtual int Id { get; set; }
        public virtual string ClassPath { get; set; }
        public virtual StoragePluginStatus Status { get; set; }
        public virtual long Version { get; set; }
    }
}
