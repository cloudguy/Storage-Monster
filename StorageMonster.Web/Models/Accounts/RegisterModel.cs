using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StorageMonster.Common.DataAnnotations;
using StorageMonster.Web.Properties;
using StorageMonster.Web.Services.Validation;
using System.Web.Mvc;

namespace StorageMonster.Web.Models.Accounts
{
	public class RegisterModel
	{
		private String _userName;
        private String _email;

		[Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("RegUserName", typeof(DisplayNameResources))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
		[RegularExpression(Constants.UserNameRegexp, ErrorMessageResourceName = "InvalidFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
		public string UserName
		{
            get { return _userName; }
            set { _userName = value != null ? value.Trim() : value; }
		}

		[Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("RegEmail", typeof(DisplayNameResources))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
		[RegularExpression(Constants.EmailRegexp, ErrorMessageResourceName = "InvalidFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
		public string Email
		{
			get{return _email;}
			set {_email = value != null ? value.Trim() : value;}
		}

		[Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [MinPasswordLength("PasswordMinLengthFormat", typeof(ValidationResources))]
        [LocalizedDisplayName("RegPassword", typeof(DisplayNameResources))]	
		public string Password { get; set; }

		[Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("RegConfirmPassword", typeof(DisplayNameResources))]
        [PropertiesMustMatch("Password", "PasswordsMustMatch", typeof(ValidationResources))]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("RegLocale", typeof(DisplayNameResources))]
		public string Locale { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("RegTimeZone", typeof(DisplayNameResources))]
        public int TimeZone { get; set; }

		public IEnumerable<SelectListItem> SupportedLocales { get; set; }
        public IEnumerable<SelectListItem> SupportedTimeZones { get; set; }

		public int MinPasswordLength {get;set;}

		public void Init(IEnumerable<SelectListItem> supportedLocales, IEnumerable<SelectListItem> supportedTimeZones, int minPasswordLength)
		{
			MinPasswordLength = minPasswordLength;
			SupportedLocales = supportedLocales;
		    SupportedTimeZones = supportedTimeZones;
		}
	}
}
