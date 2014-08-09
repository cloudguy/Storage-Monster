using System.Collections.Generic;

namespace CloudBin.Core.Domain
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual IList<UserEmail> Emails { get; set; }
        public virtual string Name { get; set; }
        public virtual string Password { get; set; }
        public virtual string Locale { get; set; }
        public virtual int TimeZone { get; set; }
        public virtual long Version { get; set; }
    }
}
