using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StorageMonster.Common;
using StorageMonster.Services;
using StorageMonster.Web.Models;
using StorageMonster.Web.Services.ActionAnnotations;
using StorageMonster.Web.Services.ActionResults;
using StorageMonster.Web.Services.Configuration;
using StorageMonster.Web.Services.Metadata;
using StorageMonster.Web.Services.Security;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace StorageMonster.Web.Controllers
{
    [TempDataTransfer]
    public abstract class BaseController : Controller
    {
        protected const string LocaleDropDownListCacheKey = "Web.LocaleDropDownListKey";
        protected const string TimeZonesDropDownListCacheKey = "Web.TimeZonesDropDownListKey";
        protected const string StoragePluginsDropDownListCacheKey = "Web.StoragePluginsDropDownListKey";
       

        protected readonly ICacheService CacheService;
        protected readonly ILocaleProvider LocaleProvider;
        protected readonly ITimeZonesProvider TimeZonesProvider;
        protected readonly IStoragePluginsService StoragePluginsService;
        protected readonly IModelMetadataExtractor ModelMetadataExtractor;

        protected IEnumerable<SelectListItem> GetSupportedLocales()
        {
            return CacheService.Get(LocaleDropDownListCacheKey, () =>
                LocaleProvider.SupportedLocales.Select(x => new SelectListItem
                {
                    Text = x.FullName,
                    Value = x.ShortName,
                    Selected = false
                }).ToArray() /*override lazy init*/);
        }

        protected IEnumerable<SelectListItem> GetSupportedTimeZones()
        {
            return CacheService.Get(TimeZonesDropDownListCacheKey, () =>
                TimeZonesProvider.GetTimezones().Select(x => new SelectListItem
                {
                    Text = x.TimeZoneName,
                    Value = x.Id.ToString(CultureInfo.InvariantCulture),
                    Selected = false
                }).ToArray() /*override lazy init*/);
        }

        protected IEnumerable<SelectListItem> GetSupportedStoragePlugins()
        {
            return CacheService.Get(StoragePluginsDropDownListCacheKey, () =>
                StoragePluginsService.GetAvailableStoragePlugins().Select(x => new SelectListItem
                {
                    Text = StoragePluginsService.GetStoragePlugin(x.Id).Name,
                    Value = x.Id.ToString(CultureInfo.InvariantCulture),
                    Selected = false
                }).ToList() /*override lazy init*/);
        }

        protected BaseController()
        {
            ModelMetadataExtractor = DependencyResolver.Current.GetService<IModelMetadataExtractor>();
            CacheService = DependencyResolver.Current.GetService<ICacheService>();
            LocaleProvider=DependencyResolver.Current.GetService<ILocaleProvider>();
            TimeZonesProvider = DependencyResolver.Current.GetService<ITimeZonesProvider>();
            StoragePluginsService = DependencyResolver.Current.GetService<IStoragePluginsService>();
        }

        protected JsonWithMetadataResult<T> JsonWithMetadata<T>(T model, JsonRequestBehavior requestBehavior) where T : BaseAjaxDataModel
        {
            return new JsonWithMetadataResult<T>(model, ModelMetadataExtractor, requestBehavior);
        }

        protected JsonWithMetadataResult<T> JsonWithMetadata<T>(T model) where T : BaseAjaxDataModel
        {
            return JsonWithMetadata(model, JsonRequestBehavior.DenyGet);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!(filterContext.HttpContext.User.Identity is Identity))
                throw new InvalidOperationException("Storage monster custom identity is supported only.");
        }

        public new Principal User
        {
            get { return (Principal)base.User; }
        }

        private const string DefaultUrlScheme = "http";
        protected string BaseSiteUrl()
        {
            var webConfig = (WebConfigurationSection)ConfigurationManager.GetSection(WebConfigurationSection.SectionLocation);
            if (webConfig.AutoDetectSiteUrl)
            {
                string scheme;
                if (Request == null || Request.Url == null)
                    scheme = DefaultUrlScheme;
                else
                    scheme = Request.Url.Scheme;
                return Url.Action("Index", "Home", null, scheme);
            }
            return webConfig.SiteUrl;
        }

        protected string FullUrlForAction(string action, string controller, object routeValues)
        {
            var webConfig = (WebConfigurationSection)ConfigurationManager.GetSection(WebConfigurationSection.SectionLocation);
            if (webConfig.AutoDetectSiteUrl)
            {
                string scheme;
                if (Request == null || Request.Url == null)
                    scheme = DefaultUrlScheme;
                else
                    scheme = Request.Url.Scheme;
                return Url.Action(action, controller, routeValues, scheme);
            }
            string url = Url.Action(action, controller, routeValues);
            if (url== null)
                throw new Exception(string.Format(CultureInfo.InvariantCulture, "Url not found for action {0} and controller {1}", action, controller));
            string relativeUrl = url.TrimStart(new[] { '~', '/' });
            return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", webConfig.SiteUrl.TrimEnd('/'), relativeUrl);
        }
    }
}
