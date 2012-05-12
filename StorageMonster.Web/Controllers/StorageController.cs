using System.Collections.Generic;
using System.Web.Mvc;
using StorageMonster.Services.Security;
using StorageMonster.Web.Models;
using StorageMonster.Web.Services;

namespace StorageMonster.Web.Controllers
{
    public class StorageController : BaseController
    {
        [AjaxOnly(JsonRequestBehavior.AllowGet)]
        [MonsterAuthorize(new[] { MonsterRoleProvider.RoleUser, MonsterRoleProvider.RoleAdmin })]
        public ActionResult List()
        {
            List<UserStorageModel> storages = new List<UserStorageModel>();
            List<UserMenuModel> accounts = new List<UserMenuModel>();
            for (int i =1;i<=5;i++)
            {
                UserStorageModel smodel = new UserStorageModel
                    {
                        Id = i,
                        Name = "storage name",
                        ImagePath = "/Content/img1"
                    };
                for (int j = 1; j <= 3; j++)
                {
                    UserMenuModel amodel = new UserMenuModel
                        {
                            //StorageId = i,
                            //AccountLogin = "Acclogin@mail.ru",
                            //FreeSpace = "2 Gb",
                            //TotalSpace = "5 Gb",
                            //Id = i,
                            //UsedSpace = "3 Gb"
                        };
                    accounts.Add(amodel);

                }
                storages.Add(smodel);
            }
            UserStorageListModel model = new UserStorageListModel
                {
                    UserStorages = storages,
                    UserAccounts = accounts
                };

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
