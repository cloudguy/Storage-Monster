using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using StorageMonster.Common;
using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Web.Models;
using StorageMonster.Web.Services.ActionAnnotations;

namespace StorageMonster.Web.Controllers
{
    public class HomeController : BaseController
    {
        [MonsterAuthorize(UserRole.Admin|UserRole.User)]
        public ActionResult Index()
        {
            SettingsModel model = new SettingsModel
                {
                    SupportedLocales = GetSupportedLocales(),
                    SupportedTimeZones = GetSupportedTimeZones(),
                    SupportedStoragePlugins = GetSupportedStoragePlugins()
                };
            return View(model);
        }

        public ActionResult CrashTest()
        {
            throw new Exception();
        }
    }
}
