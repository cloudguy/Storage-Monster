using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StorageMonster.Web.Services.Security;

namespace StorageMonster.Web.Services.Validation
{
    public class EmailLengthAttribute : BaseStringLengthAttribute
    {
        protected override int GetMinLength()
        {
            return 0;
        }

        protected override int GetMaxLength()
        {
            var service = DependencyResolver.Current.GetService<IMembershipService>();
            return service.MaxEmailLength;
        }
    }
}