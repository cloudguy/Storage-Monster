using System.ComponentModel.DataAnnotations;
using StorageMonster.Common.DataAnnotations;
using StorageMonster.Web.Properties;

namespace StorageMonster.Web.Models.Accounts
{
    public class ResetPasswordRequestModel
    {
        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("ResetPasswdEmail", typeof(DisplayNameResources))]        
        public string Email { get; set; }
    }
}