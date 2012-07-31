using System;

namespace StorageMonster.Domain
{
    public class StorageAccount
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StoragePluginId { get; set; }
        public String AccountName { get; set; }
        public DateTime Stamp { get; set; }
    }
}
