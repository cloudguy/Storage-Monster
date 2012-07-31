using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageMonster.Web.Models
{
    public class MenuActivator
    {
        public enum ActivationTypeEnum
        {
            None,
            StorageAccount,
            EditProfile,
            ListStorageAccounts
        }

        public ActivationTypeEnum ActivationType { get; set; }
        public int StorageAccountId { get; set; }
    }
}