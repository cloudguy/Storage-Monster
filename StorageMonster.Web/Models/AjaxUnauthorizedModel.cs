using System;

namespace StorageMonster.Web.Models
{
    public class AjaxUnauthorizedModel
    {
        public bool Unauthorized { get { return true; } }
        public String Redirect { get; set; }
        public String LogOnPage { get; set; }
    }
}