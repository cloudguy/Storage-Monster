using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.DB.Domain
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StorageId { get; set; }
        public String AccountServer { get; set; }
        public String AccountLogin { get; set; }
        public DateTime Stamp { get; set; }
    }
}
