using System;
using System.Collections.Generic;

namespace StorageMonster.Domain
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string Locale { get; set; }
        public virtual int TimeZone { get; set; }
        public virtual long Version { get; set; }
        public virtual IList<StorageAccount> StorageAccounts { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}
