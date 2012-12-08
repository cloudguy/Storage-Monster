using System;
using StorageMonster.Web.Services.Security;
using System.Configuration;

namespace StorageMonster.Web.Services.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UserNameRegexAttribute : BaseRegexAttribute
    {
        protected override string GetRegexPattern()
        {
            var configuration = (SecurityConfigurationSection)ConfigurationManager.GetSection(SecurityConfigurationSection.SectionLocation);
            return configuration.Memebership.UserNameRegexp;
        }
    }
}