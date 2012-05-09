using System;

namespace StorageMonster.DB.Domain
{
    public class Session
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SessionToken { get; set; }
        public string SessionAntiforgeryToken { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
