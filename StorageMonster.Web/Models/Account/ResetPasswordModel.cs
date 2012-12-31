using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StorageMonster.Web.Properties;
using StorageMonster.Web.Services.Validation;

namespace StorageMonster.Web.Models.Account
{
    public class ResetPasswordModel
    {
        public string Token { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [PasswordLength(ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = "ProfilePassword", ResourceType = typeof(DisplayNameResources))]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = "ProfileConfirmPassword", ResourceType = typeof(DisplayNameResources))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordsMustMatch", ErrorMessageResourceType = typeof(ValidationResources))]
        public string ConfirmNewPassword { get; set; }

        public int MinPasswordLength { get; set; }
    }
}