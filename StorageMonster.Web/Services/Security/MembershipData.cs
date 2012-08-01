using System;
namespace StorageMonster.Web.Services.Security
{
    public class MembershipData
    {
        public string Locale { get; set; }
        public int UserId { get; set; }       
        public DateTime Stamp { get; set; }
    }
}
