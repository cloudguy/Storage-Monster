using System.Web.Mvc;

namespace StorageMonster.Web.Controllers
{
    [OutputCache(Duration=3600, VaryByParam="none", VaryByCustom = Constants.LocaleCacheDisableKey)] //disable cache for localization
    public class BaseController : Controller
    {
    }
}
