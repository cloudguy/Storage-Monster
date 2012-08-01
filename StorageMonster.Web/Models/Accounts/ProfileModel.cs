using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageMonster.Web.Models.Accounts
{
    public class ProfileModel
    {
        public ProfileBaseModel BaseModel { get; set; }
        public ProfilePasswordModel PasswordModel { get; set; }
    }
}