using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StorageMonster.Domain;
using StorageMonster.Web.Properties;

namespace StorageMonster.Web.Models.StorageAccounts
{
    public class StorageAccountModel : BaseAjaxDataModel
    {
        public int? Id { get; set; }

        private string _accountNameTrimmed;

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof (ValidationResources))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthFormat", ErrorMessageResourceType = typeof (ValidationResources))]
        [RegularExpression(StorageAccount.StorageAccountNameRegexPattern, ErrorMessageResourceName = "InvalidFieldFormat", ErrorMessageResourceType = typeof (ValidationResources))]
        [Display(Name = "AddStorageAccountName", ResourceType = typeof (DisplayNameResources))]
        public string AccountName
        {
            get
            {
                if (_accountNameTrimmed == null)
                    return null;
                return _accountNameTrimmed.Trim();

            }
            set { _accountNameTrimmed = value == null ? null : value.Trim(); }
        }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof (ValidationResources))]
        [Display(Name = "AddStorageAccountPluginId", ResourceType = typeof (DisplayNameResources))]
        public int PluginId { get; set; }
    }
}
