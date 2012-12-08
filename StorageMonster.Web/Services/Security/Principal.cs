using System;
using System.Security.Principal;

namespace StorageMonster.Web.Services.Security
{
    [Serializable]
    public class Principal : GenericPrincipal
    {
        public Principal(Identity identity, string[] roles)
            :base(identity, roles)
        {
        }

        public new Identity Identity
        {
            get { return (Identity) base.Identity; }
        }
    }
}
