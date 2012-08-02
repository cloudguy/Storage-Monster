using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Domain
{
    public class ResetPasswordRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}