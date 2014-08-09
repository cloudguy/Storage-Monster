using System;

namespace CloudBin.Core.Domain
{
    public class UserSession
    {
        public virtual long Id { get; set; }
        public virtual Guid Token { get; set; }
        public virtual User User { get; set; }
        public virtual bool IsPersistent { get; set; }
        public virtual string UserAgent { get; set; }
// ReSharper disable InconsistentNaming
        public virtual string IPAddress { get; set; }
// ReSharper restore InconsistentNaming
        public virtual DateTimeOffset Expires { get; set; }
        public virtual DateTimeOffset SignedIn { get; set; }
    }
}
