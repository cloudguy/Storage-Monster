using System;

namespace StorageMonster.Domain
{
    [Flags]
    public enum UserRole
    {
        None = 0,
        User = 1,
        Admin = 2
    }
}
