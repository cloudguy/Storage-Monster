using System.Web.Mvc;
using CloudBin.Core;

namespace CloudBin.Web.UI.Controllers
{
    public class MainController : Controller
    {
        //private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            ILocaleProvider localeProvider = System.Web.Mvc.DependencyResolver.Current.GetService<ILocaleProvider>();
            //var repo = DependencyResolver.Current.GetService<IStoragePluginDescriptorRepository>();
            //var res = repo.Read(1);
            //Log.Error("zzzz");
            ViewData["Message"] = "Welcome to ASP.NET MVC on Mono!";
            return View();
        }

        public ActionResult Test()
        {
        //    User user = new User
        //    {
        //        Locale = "en-US",
        //        Name = "name",
        //        TimeZone = 100,
        //        Password = "zzz",
        //        Emails = new List<UserEmail>()
        //    };

        //    UserEmail ue = new UserEmail();
        //    ue.Email = "zz@qq.zz";
        //    ue.User = user;

        //    user.Emails.Add(ue);

        //    var repo = DependencyResolver.Current.GetService<IUserRepository>();
        //    repo.Create(user);


            //Log.Error("zzzz");
            ViewData["Message"] = "Welcome to ASP.NET MVC on Mono!";
            return View("Index");
        }
    }
}
