using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StorageMonster.Web.Models
{
    public class LocaleListModel
    {
        public IEnumerable<SelectListItem> SupportedLocales { get; set; }
        public string ControlName { get; set; }
        public Object HtmlAttributes { get; set; }
    }
}