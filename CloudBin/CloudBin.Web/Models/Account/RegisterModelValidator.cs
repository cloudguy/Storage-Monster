using CloudBin.Web.Resources;
using FluentValidation;

namespace CloudBin.Web.Models.Account
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
