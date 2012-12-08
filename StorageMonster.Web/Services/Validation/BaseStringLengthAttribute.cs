using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class BaseStringLengthAttribute : ValidationAttribute, IClientValidatable
    {
        protected const string DefaultErrorMessage = "Field {0} must be less or equal than {1} characters and more or equal than {2} characters";

        protected int _minLength;
        protected int _maxLength;
        private bool isInitialized;

        public BaseStringLengthAttribute()
            : base(DefaultErrorMessage)
        {
        }

        protected abstract int GetMinLength();
        protected abstract int GetMaxLength();

        protected void SetupLength()
        {
            if (!isInitialized)
            {
                _minLength = GetMinLength();
                _maxLength = GetMaxLength();
                isInitialized = true;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            SetupLength();
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, _minLength, _maxLength);
        }

        public override bool IsValid(object value)
        {
            SetupLength();
            string valueAsString = Convert.ToString(value, CultureInfo.CurrentCulture); ;
            if (string.IsNullOrEmpty(valueAsString))
                return _minLength <= 0;

            return (valueAsString.Length >= _minLength && valueAsString.Length <= _maxLength);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            SetupLength();
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "length"
            };
            if (_minLength != 0)
            {
                rule.ValidationParameters.Add("min", _minLength);
            }
            if (_maxLength != 2147483647)
            {
                rule.ValidationParameters.Add("max", _maxLength);
            }
            return new[] { rule };
        }
    }
}