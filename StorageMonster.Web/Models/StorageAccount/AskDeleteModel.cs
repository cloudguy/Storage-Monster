using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageMonster.Web.Models.StorageAccount
{
    public class AskDeleteModel
    {
        public string ReturnUrl {get;set;}
        public int StorageAccountId { get; set; }
        public String StorageAccountName { get; set; }
    }
}