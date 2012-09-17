using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using StorageMonster.Web.Properties;

namespace StorageMonster.Web.Models.StorageAccount
{
    public class AskDeleteModel
    {
        public string ReturnUrl { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldFormat", ErrorMessageResourceType = typeof(ValidationResources))]
        public int StorageAccountId { get; set; }

        public String StorageAccountName { get; set; }
    }
}