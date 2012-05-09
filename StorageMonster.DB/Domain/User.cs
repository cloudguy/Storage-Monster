using System;

namespace StorageMonster.DB.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Locale { get; set; }
        public int TimeZone { get; set; }
        public DateTime Stamp { get; set; }
    }
}
