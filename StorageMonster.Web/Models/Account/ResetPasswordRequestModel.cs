using StorageMonster.Web.Properties;
using System.ComponentModel.DataAnnotations;

namespace StorageMonster.Web.Models.Account
{
    public class ResetPasswordRequestModel
    {
        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = "ResetPasswdEmail", ResourceType = typeof(DisplayNameResources))]
        public string Email { get; set; }
    }
}