using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StorageMonster.Common;
using StorageMonster.Domain;
using StorageMonster.Services;
using StorageMonster.Web.Models.Accounts;
using StorageMonster.Web.Properties;
using StorageMonster.Web.Services.ActionAnnotations;
using StorageMonster.Web.Services.Extensions;
using StorageMonster.Web.Services.Security;

namespace StorageMonster.Web.Controllers
{
    public sealed class AccountController : BaseController
    {
        private const string LocaleDropDownListCacheKey = "Web.LocaleDropDownListKey";

        private readonly ICacheService _cacheService;
        private readonly ILocaleProvider _localeProvider;
        private readonly IMembershipService _membershipService;
        private readonly IUserService _userService;
        private readonly IFormsAuthenticationService _formsAuthService;

        public AccountController(ICacheService cacheService, 
            ILocaleProvider localeProvider,
            IMembershipService membershipService,
            IUserService userService,
            IFormsAuthenticationService formsAuthService)
        {
            _cacheService = cacheService;
            _localeProvider = localeProvider;
            _membershipService = membershipService;
            _formsAuthService = formsAuthService;
            _userService = userService;
        }
        
        public ActionResult LogOff()
        {
            _formsAuthService.SignOut();
			
            return RedirectToAction("Index", "Home");
        }        
        
        private IEnumerable<SelectListItem> GetSupportedLocales()
        {
            return _cacheService.Get(LocaleDropDownListCacheKey, () =>
                _localeProvider.SupportedLocales.Select(x => new SelectListItem
                {
                    Text = x.FullName,
                    Value = x.ShortName,
                    Selected = false
                }).ToList() /*override lazy init*/);
        }

        public ActionResult Register()
        {
			RegisterModel model = new RegisterModel();
			model.Init(GetSupportedLocales(), _membershipService.MinPasswordLength);			
            model.Init(GetSupportedLocales(), 6);	
			return View(model);
        }
        
		[MonsterAuthorize(MonsterRoleProvider.RoleUser, MonsterRoleProvider.RoleAdmin)]
		public ActionResult Edit()
		{
			ProfileModel model = new ProfileModel();
			model.Init(GetSupportedLocales(), _membershipService.MinPasswordLength);
		    Principal principal = (Principal) User;
		    Identity identity = (Identity) principal.Identity;
            User user = _userService.Load(identity.UserId);
			model.Email = user.Email;
			model.UserName = user.Name;
			model.Locale = user.Locale;
			var selectedLocale = model.SupportedLocales.Where(l => String.Equals(l.Value, model.Locale, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
			if (selectedLocale != null)
				selectedLocale.Selected = true;
			return View(model);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Register(RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				if (model == null)
					model = new RegisterModel();
				model.Init(GetSupportedLocales(), _membershipService.MinPasswordLength);
				return View(model);
			}

			model.Init(GetSupportedLocales(), _membershipService.MinPasswordLength);

			// Attempt to register the user
			MembershipCreateStatus createStatus = _membershipService.CreateUser(model.UserName, model.Password, model.Email, model.Locale);

			if (createStatus == MembershipCreateStatus.Success)
			{
				_formsAuthService.SignIn(model.Email, false /* createPersistentCookie */);
				return RedirectToAction("Index", "Home");
			}
			
            ModelState.AddModelError("_FORM", ErrorCodeToString(createStatus));

			// If we got this far, something failed, redisplay form
			return View(model);
		}

        
        public ActionResult LogOn()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            LogOnModel model = new LogOnModel();
            return View(model);
        }
        

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult LogOn(LogOnModel model, string returnUrl)
		{
            if (!ModelState.IsValid)
            {
                if (model == null)
                    model = new LogOnModel();
                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new AjaxLogOnModel
                        {
                            Success = false,
                            Html = this.RenderViewToString("LogOnFormControl", model)
                        });
                }
                return View(model);
            }


            if (!_membershipService.ValidateUser(model.Email, model.Password))
            {
                ModelState.AddModelError("_FORM", ValidationResources.UserNameOrPasswordIncorrect);
                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new AjaxLogOnModel
                        {
                            Success = false,
                            Html = this.RenderViewToString("LogOnFormControl", model)
                        });
                }
                return View(model);
            }

			_formsAuthService.SignIn(model.Email, model.RememberMe);

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new AjaxLogOnModel
                {
                    Success = true,
                });

			if (!String.IsNullOrEmpty(returnUrl))
				return Redirect(returnUrl);

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
