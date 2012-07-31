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
		protected String UserNameProtected;
        protected String EmailProtected;

		[Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("RegUserName", typeof(DisplayNameResources))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
		[RegularExpression(Constants.UserNameRegexp, ErrorMessageResourceName = "InvalidFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
		public string UserName
		{
			get{return UserNameProtected;}
			set {UserNameProtected = value != null ? value.Trim() : value;}
		}

		[Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("RegEmail", typeof(DisplayNameResources))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
		[RegularExpression(Constants.EmailRegexp, ErrorMessageResourceName = "InvalidFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
		public string Email
		{
			get{return EmailProtected;}
			set {EmailProtected = value != null ? value.Trim() : value;}
		}

		[Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(200, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
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

		public IEnumerable<SelectListItem> SupportedLocales { get; set; }

		public int MinPasswordLength {get;set;}

		public void Init(IEnumerable<SelectListItem> supportedLocales, int minPasswordLength)
		{
			MinPasswordLength = minPasswordLength;
			SupportedLocales = supportedLocales;
		}
	}
}
