using StorageMonster.Web.Services.Security;
using System.Web.Mvc;

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