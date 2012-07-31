using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StorageMonster.Common.DataAnnotations;
using StorageMonster.Domain;
using StorageMonster.Web.Properties;

namespace StorageMonster.Web.Models.StorageAccount
{
    public class StorageAccountModel
    {
        public IEnumerable<SelectListItem> StoragePlugins { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("AddStorageAccountPluginId", typeof(DisplayNameResources))]
        public int PluginId { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [RegularExpression(Constants.StorageAccountNameRegexp, ErrorMessageResourceName = "InvalidFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        [LocalizedDisplayName("AddStorageAccountName", typeof(DisplayNameResources))]
        public string AccountName
        {
            get
            {
                if (_accountNameTrimmed == null)
                    return null;
                return _accountNameTrimmed.Trim();
               
            } 
            set 
            {
                _accountNameTrimmed = value == null ? null : value.Trim();
            }
        }

        private string _accountNameTrimmed;
    }
}
