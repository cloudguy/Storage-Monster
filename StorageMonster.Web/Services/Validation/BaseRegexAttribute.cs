using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class BaseRegexAttribute : ValidationAttribute, IClientValidatable
    {
        protected const string DefaultErrorMessage = "Field {0} is invalid. Should match patern {1}";
        protected string Pattern;
        protected Regex Regex;

        public BaseRegexAttribute()
            : base(DefaultErrorMessage)
        {
        }

        protected abstract string GetRegexPattern();

        protected void SetupRegex()
        {
            if (Regex != null)
                return;
            Pattern = GetRegexPattern();
            if (string.IsNullOrEmpty(Pattern))
                throw new InvalidOperationException("Empty pattern");
            Regex = new Regex(Pattern);
            if (Regex == null)
                throw new InvalidOperationException("Can not get validation regex");
        }

        public override string FormatErrorMessage(string name)
        {
            SetupRegex();
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Pattern);
        }

        public override bool IsValid(object value)
        {
            SetupRegex();
            string input = Convert.ToString(value, CultureInfo.CurrentCulture);
            if (string.IsNullOrEmpty(input))
                return true;
            Match match = Regex.Match(input);
            if (match.Success && match.Index == 0)
                return match.Length == input.Length;
            return false;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            SetupRegex();
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "regex"
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            return new[] { rule };
        }
    }
}
