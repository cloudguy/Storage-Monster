using StorageMonster.Common;
using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Web.Models.Account;
using StorageMonster.Web.Properties;
using StorageMonster.Web.Services;
using StorageMonster.Web.Services.Security;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace StorageMonster.Web.Controllers
{
    public class AccountController : BaseController
    {
        private const string LocaleDropDownListCacheKey = "Web.LocaleDropDownListKey";

        private readonly ICacheService _cacheService;
        private readonly ILocaleProvider _localeProvider;
        private readonly IMembershipService _membershipService;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;
        private readonly ITimeZonesProvider _timeZonesProvider;

        public AccountController(ICacheService cacheService,
            ILocaleProvider localeProvider,
            IMembershipService membershipService,
            IUserService userService,
            IAuthenticationService authService,
            ITimeZonesProvider timeZonesProvider)
        {
            _cacheService = cacheService;
            _localeProvider = localeProvider;
            _membershipService = membershipService;
            _authService = authService;
            _userService = userService;
            _timeZonesProvider = timeZonesProvider;
        }

        private IEnumerable<SelectListItem> GetSupportedLocales()
        {
#warning pref locale
            return _cacheService.Get(LocaleDropDownListCacheKey, () =>
                _localeProvider.SupportedLocales.Select(x => new SelectListItem
                {
                    Text = x.FullName,
                    Value = x.ShortName,
                    Selected = false
                }).ToArray() /*override lazy init*/);
        }

        private IEnumerable<SelectListItem> GetSupportedTimeZones()
        {
            return _timeZonesProvider.GetTimezones().Select(x => new SelectListItem
            {
                Text = x.TimeZoneName,
                Value = x.Id.ToString(CultureInfo.InvariantCulture),
                Selected = false
            });
        }

        public ActionResult LogOn(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            LogOnModel model = new LogOnModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult LogOn(LogOnModel model)
        {
            
            if (!ModelState.IsValid)
            {
                if (model == null)
                    model = new LogOnModel();
                if (HttpContext.Request.IsAjaxRequest())
                    return AuthorizationHelper.GetAuthAjaxResult(ControllerContext, model, false);
                return View(model);
            }

            User user;
            if (!_membershipService.ValidateUser(model.Email, model.Password, out user))
            {
                ModelState.AddModelError("_FORM", ValidationResources.UserNameOrPasswordIncorrect);
                if (HttpContext.Request.IsAjaxRequest())
                    return AuthorizationHelper.GetAuthAjaxResult(ControllerContext, model, false);
                return View(model);
            }

            _authService.SignIn(user, model.RememberMe);

            if (HttpContext.Request.IsAjaxRequest())
                return AuthorizationHelper.GetAuthAjaxResult(ControllerContext, model, true);

            if (Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            RegisterModel model = new RegisterModel();
            model.Init(GetSupportedLocales(), GetSupportedTimeZones(), _membershipService.MinPasswordLength);
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model == null)
                    model = new RegisterModel();
                model.Init(GetSupportedLocales(), GetSupportedTimeZones(), _membershipService.MinPasswordLength);
                return View(model);
            }

            User user;
            // Attempt to register the user
            MembershipCreateStatus createStatus = _membershipService.CreateUser(model.Email, model.Password, model.UserName, model.Locale, model.TimeZone, out user);

            if (createStatus == MembershipCreateStatus.Success)
            {
                _authService.SignIn(user, false /* createPersistentCookie */);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("_FORM", ErrorCodeToString(createStatus));

            model.Init(GetSupportedLocales(), GetSupportedTimeZones(), _membershipService.MinPasswordLength);
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            _authService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return ValidationResources.RegDuplicateUserName;

                case MembershipCreateStatus.DuplicateEmail:
                    return ValidationResources.RegDuplicateEmail;

                case MembershipCreateStatus.InvalidPassword:
                    return ValidationResources.RegInvalidPassword;

                case MembershipCreateStatus.InvalidEmail:
                    return ValidationResources.RegInvalidEmail;

                case MembershipCreateStatus.InvalidAnswer:
                    return ValidationResources.RegInvalidAnswer;

                case MembershipCreateStatus.InvalidQuestion:
                    return ValidationResources.RegInvalidQuestion;

                case MembershipCreateStatus.InvalidUserName:
                    return ValidationResources.RegInvalidUserName;

                case MembershipCreateStatus.ProviderError:
                    return ValidationResources.RegProviderError;

                case MembershipCreateStatus.UserRejected:
                    return ValidationResources.RegUserRejected;

                default:
                    return ValidationResources.RegUnknownError;
            }
        }
    }
}
