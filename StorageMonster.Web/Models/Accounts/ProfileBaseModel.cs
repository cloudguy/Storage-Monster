using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StorageMonster.Common.DataAnnotations;
using StorageMonster.Web.Properties;
using System.Web.Mvc;

namespace StorageMonster.Web.Models.Accounts
{
	public class ProfileBaseModel
	{
		private String _userName;

        [LocalizedDisplayName("ProfileEmail", typeof(DisplayNameResources))]		
		public string Email { get; set; }
		
		[Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("ProfileUserName", typeof(DisplayNameResources))]
		[StringLength(100, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
		[RegularExpression(Constants.UserNameRegexp, ErrorMessageResourceName = "InvalidFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
		public string UserName
		{
			get { return _userName; }
			set { _userName = value != null ? value.Trim() : value; }
		}

		[Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("ProfileLocale", typeof(DisplayNameResources))]
		public string Locale { get; set; }

        [Required(ErrorMessageResourceName = "StampRequired", ErrorMessageResourceType = typeof(ValidationResources))]
        public long Stamp { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("ProfileTimeZone", typeof(DisplayNameResources))]
        public int TimeZone { get; set; }

		public IEnumerable<SelectListItem> SupportedLocales { get; set; }
        public IEnumerable<SelectListItem> SupportedTimeZones { get; set; }

		public void Init(IEnumerable<SelectListItem> supportedLocales, IEnumerable<SelectListItem> supportedTimeZones)
		{			
			SupportedLocales = supportedLocales;
		    SupportedTimeZones = supportedTimeZones;
		}
	}
}
