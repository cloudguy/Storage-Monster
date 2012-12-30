using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class BaseStringLengthAttribute : ValidationAttribute, IClientValidatable
    {
        protected const string DefaultErrorMessage = "Field {0} must be less or equal than {1} characters and more or equal than {2} characters";

        protected int MinLength;
        protected int MaxLength;
        private bool _isInitialized;

        public BaseStringLengthAttribute()
            : base(DefaultErrorMessage)
        {
        }

        protected abstract int GetMinLength();
        protected abstract int GetMaxLength();

        protected void SetupLength()
        {
            if (!_isInitialized)
            {
                MinLength = GetMinLength();
                MaxLength = GetMaxLength();
                _isInitialized = true;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            SetupLength();
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, MinLength, MaxLength);
        }

        public override bool IsValid(object value)
        {
            SetupLength();
            string valueAsString = Convert.ToString(value, CultureInfo.CurrentCulture);
            if (string.IsNullOrEmpty(valueAsString))
                return MinLength <= 0;

            return (valueAsString.Length >= MinLength && valueAsString.Length <= MaxLength);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            SetupLength();
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "length"
            };
            if (MinLength != 0)
            {
                rule.ValidationParameters.Add("min", MinLength);
            }
            if (MaxLength != 2147483647)
            {
                rule.ValidationParameters.Add("max", MaxLength);
            }
            return new[] { rule };
        }
    }
}