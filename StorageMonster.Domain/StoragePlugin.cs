using System;

namespace StorageMonster.Domain
{
    public class StoragePlugin
    {
        public int Id { get; set; }
        public string ClassPath { get; set; }
        public int Status { get; set; }
        public DateTime Stamp { get; set; }
    }
}
