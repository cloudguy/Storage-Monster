using System.ComponentModel.DataAnnotations;
using StorageMonster.Common.DataAnnotations;
using StorageMonster.Web.Properties;
using StorageMonster.Web.Services.Validation;

namespace StorageMonster.Web.Models.Accounts
{
    public class ResetPasswordModel
    {
        public string Token { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinPasswordLength("PasswordMinLengthFormat", typeof(ValidationResources), CanBeNull = true)]
        [LocalizedDisplayName("ProfilePassword", typeof(DisplayNameResources))]
        public string NewPassword { get; set; }        

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("ProfileConfirmPassword", typeof(DisplayNameResources))]
        [PropertiesMustMatch("NewPassword", "PasswordsMustMatch", typeof(ValidationResources))]
        public string ConfirmNewPassword { get; set; }
        
        public int MinPasswordLength { get; set; }

        public void Init(int minPasswordLength)
        {
            MinPasswordLength = minPasswordLength;
        }
    }
}