using System;

namespace StorageMonster.DB.Domain
{
    public sealed class IdAndStamp
    {
        public int Id { get; set; }
        public DateTime Stamp { get; set; }
    }
}
