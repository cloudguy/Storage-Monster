using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StorageMonster.Common.DataAnnotations;
using StorageMonster.Web.Properties;
using System.Web.Mvc;
using StorageMonster.Web.Services.Validation;

namespace StorageMonster.Web.Models.Accounts
{
	public class ProfileBaseModel
	{
		protected String UserNameProtected;

        [LocalizedDisplayName("ProfileEmail", typeof(DisplayNameResources))]		
		public string Email { get; set; }
		
		[Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("ProfileUserName", typeof(DisplayNameResources))]
		[StringLength(100, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
		[RegularExpression(Constants.UserNameRegexp, ErrorMessageResourceName = "InvalidFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
		public string UserName
		{
			get { return UserNameProtected; }
			set { UserNameProtected = value != null ? value.Trim() : value; }
		}

		[Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("ProfileLocale", typeof(DisplayNameResources))]
		public string Locale { get; set; }

		public IEnumerable<SelectListItem> SupportedLocales { get; set; }
		

		public void Init(IEnumerable<SelectListItem> supportedLocales)
		{			
			SupportedLocales = supportedLocales;
		}
	}
}
