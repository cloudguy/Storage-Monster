using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Web.Models;
using StorageMonster.Web.Services.ActionAnnotations;
using StorageMonster.Web.Services.Extensions;
using StorageMonster.Web.Services.Security;
using StorageMonster.Web.Properties;


namespace StorageMonster.Web.Controllers
{
    public class HomeController : BaseController
    {
        protected IStorageAccountService AccountService;
        protected IStoragePluginsService StorageService;
        public HomeController(IStorageAccountService accountService, IStoragePluginsService storageService)
        {
            AccountService = accountService;
            StorageService = storageService;
        }

        public ActionResult BadRequest()
        {
            return View();
        }

        [MonsterAuthorize(MonsterRoleProvider.RoleUser, MonsterRoleProvider.RoleAdmin)]
        public ActionResult Index()
        {
            return View();
        }
       
        [AcceptVerbs(HttpVerbs.Post)]
        [MonsterAuthorize(MonsterRoleProvider.RoleUser, MonsterRoleProvider.RoleAdmin)]
        public ActionResult SendMail()
        {
            //var engine = IocContainer.Instance.Resolve<ITemplateEngine>();

            //string emailTemplate = MailResources.RestorePasswordMail;

            //string template = engine.TransformTemplate(new Dictionary<string, object> { { "token", "fffd-7777-855-664" } }, emailTemplate);

           /* var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.To.Add("cloudguy@yandex.ru");
            mailMessage.From = new MailAddress("cloudguy@apphb.com");
            mailMessage.Subject = "test appharbor mail";
            mailMessage.Body = "test appharbor body";

            var smtpClient = new SmtpClient();
            //smtpClient.Host = "smtp.mailgun.org";
            //smtpClient.Credentials = new NetworkCredential("postmaster@app1090.mailgun.org", "44nqd7xsopo7");
            //smtpClient.Port = 587;
            //smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);*/
            return View("Index");
            //postmaster@app1090.mailgun.org
        }

        [AjaxOnly(JsonRequestBehavior.AllowGet)]
        [MonsterAuthorize(MonsterRoleProvider.RoleUser, MonsterRoleProvider.RoleAdmin)]
        public ActionResult UserMenu()
        {
            Identity identity = (Identity)HttpContext.User.Identity;
            StorageAccountsCollection accountsCollection = new StorageAccountsCollection().Init(AccountService, StorageService, identity.UserId);
            UserMenuModel menuModel = new UserMenuModel
            {
                StorageAccountsCollection = accountsCollection
            };
            string viewContent = this.RenderViewToString("UserMenu", menuModel);
            return Json(new { Menu = viewContent }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotFound()
        {
            return View();
        }

#if DEBUG

        [MonsterAuthorize(MonsterRoleProvider.RoleUser)]
        public ActionResult TestUser()
        {
            return new EmptyResult();
        }


        [MonsterAuthorize(MonsterRoleProvider.RoleAdmin)]
        public ActionResult TestAdmin()
        {
            return new EmptyResult();
        }

        public ActionResult CrashTest()
        {
            throw new Exception("Crash test");
        }
#endif
    }
}
