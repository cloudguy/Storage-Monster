using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Validation
{
    public class MinStringLengthValidator : DataAnnotationsModelValidator<MinStringLengthAttribute>
    {
        public MinStringLengthValidator(ModelMetadata metadata, ControllerContext context, MinStringLengthAttribute attribute)
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

