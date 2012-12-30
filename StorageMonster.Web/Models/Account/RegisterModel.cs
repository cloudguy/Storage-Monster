using StorageMonster.Web.Properties;
using StorageMonster.Web.Services.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StorageMonster.Web.Models.Account
{
    public class RegisterModel
    {
        private String _userName;
        private String _email;

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = "RegUserName", ResourceType = typeof(DisplayNameResources))]
        [UserNameLength(ErrorMessageResourceName = "StringLengthLessThanFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [UserNameRegex(ErrorMessageResourceName = "InvalidFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value != null ? value.Trim() : null; }
        }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = "RegEmail", ResourceType = typeof(DisplayNameResources))]
        [EmailLength(ErrorMessageResourceName = "StringLengthLessThanFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [EmailRegex(ErrorMessageResourceName = "InvalidFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public string Email
        {
            get { return _email; }
            set { _email = value != null ? value.Trim() : null; }
        }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [PasswordLength(ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = "RegPassword", ResourceType = typeof(DisplayNameResources))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = "RegConfirmPassword", ResourceType = typeof(DisplayNameResources))]
        [Compare("Password", ErrorMessageResourceName= "PasswordsMustMatch", ErrorMessageResourceType = typeof(ValidationResources))]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = "RegLocale", ResourceType = typeof(DisplayNameResources))]
        public string Locale { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Display(Name = "RegTimeZone", ResourceType = typeof(DisplayNameResources))]
        public int TimeZone { get; set; }

        public IEnumerable<SelectListItem> SupportedLocales { get; set; }
        public IEnumerable<SelectListItem> SupportedTimeZones { get; set; }

        public int MinPasswordLength { get; set; }

        public void Init(IEnumerable<SelectListItem> supportedLocales, IEnumerable<SelectListItem> supportedTimeZones, int minPasswordLength)
        {
            MinPasswordLength = minPasswordLength;
            SupportedLocales = supportedLocales;
            SupportedTimeZones = supportedTimeZones;
        }
    }
}