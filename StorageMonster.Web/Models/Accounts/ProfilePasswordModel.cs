using System.ComponentModel.DataAnnotations;
using StorageMonster.Web.Services.Validation;
using StorageMonster.Common.DataAnnotations;
using StorageMonster.Web.Properties;

namespace StorageMonster.Web.Models.Accounts
{
    public class ProfilePasswordModel
    {
        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinPasswordLength("PasswordMinLengthFormat", typeof(ValidationResources), CanBeNull = true)]
        [LocalizedDisplayName("ProfilePassword", typeof(DisplayNameResources))]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("ProfileOldPassword", typeof(DisplayNameResources))]        
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("ProfileConfirmPassword", typeof(DisplayNameResources))]
        [PropertiesMustMatch("NewPassword", "PasswordsMustMatch", typeof(ValidationResources))]
        public string ConfirmNewPassword { get; set; }

        [Required(ErrorMessageResourceName = "StampRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public long Stamp { get; set; }

        public int MinPasswordLength { get; set; }

        public void Init(int minPasswordLength)
        {
            MinPasswordLength = minPasswordLength;            
        }
    }
}