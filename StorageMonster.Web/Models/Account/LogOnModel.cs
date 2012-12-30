using System;
using System.ComponentModel.DataAnnotations;
using StorageMonster.Web.Properties;

namespace StorageMonster.Web.Models.Account
{
    public class LogOnModel
    {
        public String ReturnUrl { get; set; }

        private String _email;

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = "LogOnEmail", ResourceType = typeof(DisplayNameResources))]
        public string Email
        {
            get { return _email; }
            set { _email = value != null ? value.Trim() : null; }
        }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = "LogOnPassword", ResourceType = typeof(DisplayNameResources))]
        public string Password { get; set; }

        [Display(Name = "LogOnRememberMe", ResourceType = typeof(DisplayNameResources))]
        public bool RememberMe { get; set; }
    }
}