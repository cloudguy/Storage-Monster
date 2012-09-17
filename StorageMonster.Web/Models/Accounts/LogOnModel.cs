using System;
using System.ComponentModel.DataAnnotations;
using StorageMonster.Common.DataAnnotations;
using StorageMonster.Web.Properties;


namespace StorageMonster.Web.Models.Accounts
{
    public class LogOnModel
    {
        public String ReturnUrl { get; set; }

        private String _email;

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("LogOnEmail", typeof(DisplayNameResources))]
        public string Email
        {
            get { return _email; }
            set { _email = value != null ? value.Trim() : value; }
        }
        
        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("LogOnPassword", typeof(DisplayNameResources))]
        public string Password { get; set; }

        [LocalizedDisplayName("LogOnRememberMe", typeof(DisplayNameResources))]
        public bool RememberMe { get; set; }
    }
}
