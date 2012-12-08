using StorageMonster.Web.Services.Security;
using System;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PasswordLengthAttribute : BaseStringLengthAttribute
    {
        protected override int GetMinLength()
        {
            var service = DependencyResolver.Current.GetService<IMembershipService>();
            return service.MinPasswordLength;
        }

        protected override int GetMaxLength()
        {
            var service = DependencyResolver.Current.GetService<IMembershipService>();
            return service.MaxPasswordLength;
        }
    }

}