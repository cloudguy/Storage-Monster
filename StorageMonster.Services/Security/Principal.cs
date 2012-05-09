using System;
using System.Security.Principal;

namespace StorageMonster.Services.Security
{
    [Serializable]
    public class Principal : GenericPrincipal
    {
        public Principal(Identity identity, string[] roles)
            :base(identity, roles)
        {
        }
    }
}
