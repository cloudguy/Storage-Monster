using System;

namespace StorageMonster.DB.Domain
{
    public class Storage
    {
        public int Id { get; set; }
        public string ClassPath { get; set; }
        public int Status { get; set; }
        public DateTime Stamp { get; set; }
    }
}
