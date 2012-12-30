using System;

namespace StorageMonster.Web.Models
{
    public class AjaxAuthModel
    {
        public bool Authorized { get; set; }
        public String LogonUrl { get; set; }
        public String LogOnPage { get; set; }
    }
}