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
using StorageMonster.Web.Models;
using System.Globalization;

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
        [MenuActivatorAttribute(MenuActivator.ActivationTypeEnum.EditProfile)]
		public ActionResult Edit()
		{
            ProfileBaseModel baseModel = null;
            ProfileModel model = new ProfileModel 
            {
                BaseModel = GetProfileBaseModel(ref baseModel),
                PasswordModel = GetProfilePasswordModel() 
            };
            return View(model);
		}

        private ProfilePasswordModel GetProfilePasswordModel()
        {
            ProfilePasswordModel passwdModel = new ProfilePasswordModel();
            passwdModel.Init(_membershipService.MinPasswordLength);
            return passwdModel;
        }

        private ProfileBaseModel GetProfileBaseModel(ref ProfileBaseModel baseModel)
        {
            if (baseModel == null)
            {
                baseModel = new ProfileBaseModel();
                Principal principal = (Principal)User;
                Identity identity = (Identity)principal.Identity;
                User user = _userService.Load(identity.UserId);
                baseModel.Email = user.Email;
                baseModel.UserName = user.Name;
                baseModel.Locale = user.Locale;
                ViewData.Add(Constants.StampFormKey, user.Stamp.ToBinary());
            }

            ProfileBaseModel tmpModel = baseModel;
            baseModel.Init(GetSupportedLocales());
            var selectedLocale = baseModel.SupportedLocales.Where(l => String.Equals(l.Value, tmpModel.Locale, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (selectedLocale != null)
                selectedLocale.Selected = true;
            return baseModel;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken(Salt = Constants.Salt_Account_Edit)]
        [HandleError(ExceptionType = typeof(HttpAntiForgeryException), View = "Forbidden")]
        [MenuActivatorAttribute(MenuActivator.ActivationTypeEnum.EditProfile)]
        public ActionResult Edit(ProfileBaseModel baseModel)
        {
#warning add locale cookie
            baseModel = GetProfileBaseModel(ref baseModel);

            long stamp;
            if (!long.TryParse(Request.Form[Constants.StampFormKey], NumberStyles.Integer, CultureInfo.InvariantCulture, out stamp))
            {
                ModelState.AddModelError("user_stamp", ValidationResources.BadRequestError);
                return View();
            }
            

            if (ModelState.IsValid)
            {
                Principal principal = (Principal)User;
                Identity identity = (Identity)principal.Identity;

                try
                {
                    User user = _membershipService.UpdateUser(identity.UserId, baseModel.UserName, baseModel.Locale, DateTime.FromBinary(stamp));
                    stamp = user.Stamp.ToBinary();
                    LocaleData locale = _localeProvider.GetCultureByName(baseModel.Locale);
                    _localeProvider.SetThreadLocale(locale);
                    identity.Name = user.Name;
                    identity.Locale = locale.ShortName;
                    ViewData.AddRequestSuccessMessage(ValidationResources.ProfileUpdateSuccessMessage);
                    
                }
                catch (StaleObjectException)
                {
                    ModelState.AddModelError("profile", ValidationResources.AccountStalled);
                }
                catch (ObjectNotExistsException)
                {
                    ModelState.AddModelError("profile", ValidationResources.AccountNotFound);
                }
            }

            ProfileModel model = new ProfileModel
            {
                BaseModel = baseModel,
                PasswordModel = GetProfilePasswordModel()
            };
            ViewData.Add(Constants.StampFormKey, stamp);
            return View("Edit", model);
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
