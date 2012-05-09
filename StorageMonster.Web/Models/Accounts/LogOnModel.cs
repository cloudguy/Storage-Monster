using System;
using System.ComponentModel.DataAnnotations;
using StorageMonster.Web.Properties;
using StorageMonster.Web.Services;

namespace StorageMonster.Web.Models.Accounts
{
    public class LogOnModel
    {
        protected String EmailProtected;

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("LogOnEmail", typeof(DisplayNameResources))]
        public string Email
        {
            get { return EmailProtected; }
            set { EmailProtected = value != null ? value.Trim() : value; }
        }
        
        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("LogOnPassword", typeof(DisplayNameResources))]
        public string Password { get; set; }

        [LocalizedDisplayName("LogOnRememberMe", typeof(DisplayNameResources))]
        public bool RememberMe { get; set; }
    }
}
