using CloudBin.Web.UI.Resources;
using FluentValidation;

namespace CloudBin.Web.UI.Models.Account
{
    public sealed class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithLocalizedMessage(() => ValidationResources.Required)
                .Length(6,8).WithMessage("ololol");

            RuleFor(x => x.Email)
                .NotEmpty().WithLocalizedMessage(() => ValidationResources.Required);
        }
    }
}
