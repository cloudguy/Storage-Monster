using System;
using StorageMonster.Web.Services.Security;
using System.Configuration;

namespace StorageMonster.Web.Services.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EmailRegexAttribute : BaseRegexAttribute
    {
        protected override string GetRegexPattern()
        {
            var configuration = (AuthConfigurationSection)ConfigurationManager.GetSection(AuthConfigurationSection.SectionLocation);
            return configuration.Memebership.EmailRegexp;
        }
    }
}