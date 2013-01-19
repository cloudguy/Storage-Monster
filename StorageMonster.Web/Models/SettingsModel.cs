using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Models
{
    public class SettingsModel
    {
        public IEnumerable<SelectListItem> SupportedLocales { get; set; }
        public IEnumerable<SelectListItem> SupportedTimeZones { get; set; }
        public IEnumerable<SelectListItem> SupportedStoragePlugins { get; set; }
    }
}