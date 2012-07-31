using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using StorageMonster.Common.DataAnnotations;

namespace StorageMonster.Plugin.WebDav
{
    public class WebDavConfigurationModel
    {
        [StringLength(100, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "RequiredFieldFormat")]
        [MonsterInputBox(Multiline = false, ShowOrder = 1)]
        [LocalizedDisplayName("ServerUrl", typeof(DisplayResources))]
		public String ServerUrl { get; set; }

		[StringLength(100, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "RequiredFieldFormat")]
        [LocalizedDisplayName("AccountLogin", typeof(DisplayResources))]
        [MonsterInputBox(Multiline = false, ShowOrder = 2)]
		public String AccountLogin { get; set; }

        [StringLength(300, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(ValidationResources), ErrorMessageResourceName = "RequiredFieldFormat")]
        [LocalizedDisplayName("AccountPassword", typeof(DisplayResources))]
        [MonsterPasswordBox(ShowOrder = 3)]
		public String AccountPassword { get; set; }       
	}
}
