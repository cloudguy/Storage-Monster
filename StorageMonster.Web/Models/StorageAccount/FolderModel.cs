using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StorageMonster.Plugin;
using StorageMonster.Utilities.Serialization;
using StorageMonster.Web.Services;
using StorageMonster.Domain;

namespace StorageMonster.Web.Models.StorageAccount
{
    public class FolderModel
    {       
        public FolderModel()
        {            
        }
        public StorageFolderResult Content { get; set; }
        public StorageMonster.Domain.StorageAccount StorageAccount { get; set; }
    }
}