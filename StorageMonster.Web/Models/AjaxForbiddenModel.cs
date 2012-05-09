using System;

namespace StorageMonster.Web.Models
{
    public class AjaxForbiddenModel
    {
        public bool Forbidden { get { return true; } }
        public String Redirect { get; set; }
    }
}