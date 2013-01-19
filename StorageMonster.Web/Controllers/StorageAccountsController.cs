using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StorageMonster.Common;
using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Web.Models.StorageAccounts;
using StorageMonster.Web.Services.ActionAnnotations;

namespace StorageMonster.Web.Controllers
{
    public class StorageAccountsController : BaseController
    {
        [MonsterAuthorize(UserRole.Admin | UserRole.User)]
        public ActionResult Create()
        {
            StorageAccountModel model = new StorageAccountModel();
            return JsonWithMetadata(model, JsonRequestBehavior.AllowGet);
        }

        [MonsterAuthorize(UserRole.Admin | UserRole.User)]
        public ActionResult List()
        {
            StorageAccountsCollection model = new StorageAccountsCollection();
            throw new NotImplementedException();
        }
    }
}



