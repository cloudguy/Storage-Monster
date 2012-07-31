using System;

namespace StorageMonster.Domain
{
    public class Session
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
