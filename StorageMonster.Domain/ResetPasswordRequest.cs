using System;

namespace StorageMonster.Domain
{
    public class ResetPasswordRequest
    {
        public virtual int Id { get; set; }
        public virtual User User { get; set; }
        public virtual string Token { get; set; }
        public virtual DateTimeOffset Expires { get; set; }
    }
}