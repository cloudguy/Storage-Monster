using System.Web.Mvc;

namespace StorageMonster.Web.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "none")] //disable cache for localization
    public class BaseController : Controller
    {
    }
}
