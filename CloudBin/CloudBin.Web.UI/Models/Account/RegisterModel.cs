using FluentValidation.Attributes;

namespace CloudBin.Web.UI.Models.Account
{
    [Validator(typeof(RegisterModelValidator))]
    public sealed class RegisterModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}