using System.Collections.Generic;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Validation
{
    public class MinPasswordLengthValidator : DataAnnotationsModelValidator<MinPasswordLengthAttribute>
    {
        public MinPasswordLengthValidator(ModelMetadata metadata, ControllerContext context, MinPasswordLengthAttribute attribute)
            : base(metadata, context, attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = Attribute.FormatErrorMessage(Metadata.DisplayName),
                ValidationType = "minstrlength"
            };
            rule.ValidationParameters.Add("minlength", Attribute.MinCharacters);

            return new[] { rule };
        }
    }
}

