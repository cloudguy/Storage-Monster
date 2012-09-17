using System;
using System.Collections.Generic;
using System.Web.Mvc;
using StorageMonster.Common.DataAnnotations;

namespace StorageMonster.Web.Services.Validation
{
    public class PropertiesMustMatchValidator : DataAnnotationsModelValidator<PropertiesMustMatchAttribute>
    {
        public PropertiesMustMatchValidator(ModelMetadata metadata, ControllerContext context, PropertiesMustMatchAttribute attribute)
            : base(metadata, context, attribute)
        {
        }
        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var propertyToMatch = Metadata.ContainerType.GetProperty(Attribute.PropertyNameToMatch);
            if (propertyToMatch != null)
            {
                var valueToMatch = propertyToMatch.GetValue(container, null);
                var value = Metadata.Model;

                bool valid;
                if (value is string && valueToMatch is string)
                    valid = string.Compare((string)value, (string)valueToMatch, StringComparison.Ordinal) == 0;
                else
                    valid = Equals(value, valueToMatch);
                if (!valid)
                {
                    yield return new ModelValidationResult { Message = ErrorMessage };
                }
            }
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = Attribute.FormatErrorMessage(Metadata.DisplayName),
                ValidationType = "propmatch"
            };
            rule.ValidationParameters.Add("prop_id", Attribute.PropertyNameToMatch);

            return new[] { rule };
        }
    }
}
