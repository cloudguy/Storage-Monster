using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Controllers
{
    public class StorageAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class AccountsModel
    {
        public IEnumerable<StorageAccount> Accounts { get; set; }
    }
    public class StorageAccountsController : Controller
    {
        public ActionResult List()
        {
            var accounts = new List<StorageAccount>
                {
                    new StorageAccount {Id =1, Name = "Name1"},
                    new StorageAccount {Id =2, Name = "Name2"},
                };
            return Json(new AccountsModel {Accounts = accounts}, JsonRequestBehavior.AllowGet);
        }

    }
}
