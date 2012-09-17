using System;
using System.Collections.Generic;
using System.Linq;
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
using Common.Logging;
using System.Net;

namespace StorageMonster.Web.Controllers
{
    public sealed class AccountController : BaseController
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AccountController));

        private const string LocaleDropDownListCacheKey = "Web.LocaleDropDownListKey";

        private readonly ICacheService _cacheService;
        private readonly ILocaleProvider _localeProvider;
        private readonly IMembershipService _membershipService;
        private readonly IUserService _userService;
        private readonly IFormsAuthenticationService _formsAuthService;
        private readonly ITimeZonesProvider _timeZonesProvider;

        public AccountController(ICacheService cacheService, 
            ILocaleProvider localeProvider,
            IMembershipService membershipService,
            IUserService userService,
            IFormsAuthenticationService formsAuthService,
            ITimeZonesProvider timeZonesProvider)
        {
            _cacheService = cacheService;
            _localeProvider = localeProvider;
            _membershipService = membershipService;
            _formsAuthService = formsAuthService;
            _userService = userService;
            _timeZonesProvider = timeZonesProvider;
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

        public ActionResult Register()
        {
			RegisterModel model = new RegisterModel();
			model.Init(GetSupportedLocales(), GetSupportedTimeZones(), _membershipService.MinPasswordLength);			
			return View(model);
        }

        public ActionResult ResetPasswordRequest()
        {
            return View(new ResetPasswordRequestModel());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {                
                try
                {
                    _membershipService.ChangePassword(model.Token, model.NewPassword);
                    ViewData.AddRequestSuccessMessage(SuccessMessagesResources.PasswordChangedInfo);
                    return View();
                }
                catch (InvalidPasswordTokenException ex)
                {
                    throw new HttpException((int)HttpStatusCode.NotFound, "not found", ex);
                }
                catch (InvalidPasswordException)
                {
                    ModelState.AddModelError("NewPassword", ValidationResources.RegInvalidPassword);
                }
                catch (UserNotExistsException)
                {
                    ModelState.AddModelError("profile", ValidationResources.AccountNotFound);
                    return View();
                }
                catch (StaleUserException)
                {
                    ModelState.AddModelError("profile", ValidationResources.AccountStalled);
                    return View();
                }                
            }
            return View(model);           
        }

        [ValidateInput(false)]
        public ActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new HttpException((int)HttpStatusCode.NotFound, "not found");

            ResetPasswordRequest request = _membershipService.GetActivePasswordResetRequestByToken(token);

            if (request == null)
                throw new HttpException((int)HttpStatusCode.NotFound, "not found");

            ResetPasswordModel model = new ResetPasswordModel();
            model.Init(_membershipService.MinPasswordLength);
            model.Token = request.Token;

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult ResetPasswordRequest(ResetPasswordRequestModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _membershipService.RequestPasswordReset(model.Email, BaseSiteUrl(), token => FullUrlForAction("ResetPassword", "Account", new { token }));
                    ViewData.AddRequestSuccessMessage(SuccessMessagesResources.ResetPasswdRequestSentInfo);
                    return View();
                }
                catch (UserNotExistsException)
                {
                    ModelState.AddModelError("email", ValidationResources.EmailNotFoundError);
                }
                catch (DeliveryException ex)
                {
                    Logger.Error(ex);
                    ModelState.AddModelError("delivery", ValidationResources.ResetInstructionSendingFailedError);
                }
            }
            if (model == null)
                model = new ResetPasswordRequestModel();
            return View(model);
        }

        
        private ProfilePasswordModel InitProfilePasswordModel(ref ProfilePasswordModel passwdModel, User user)
        {
            if (passwdModel == null)
                passwdModel = new ProfilePasswordModel();
            if (user != null)
                passwdModel.Stamp = user.Stamp.ToBinary();
            passwdModel.Init(_membershipService.MinPasswordLength);
            return passwdModel;
        }

        private ProfileBaseModel InitBaseProfileModel(ref ProfileBaseModel baseModel, User user)
        {
            if (baseModel == null)
                baseModel = new ProfileBaseModel();

            if (user != null)
            {
                baseModel.Email = user.Email;
                baseModel.UserName = user.Name;
                baseModel.Locale = user.Locale;
                baseModel.Stamp = user.Stamp.ToBinary();
                baseModel.TimeZone = user.TimeZone;
            }

            ProfileBaseModel tmpModel = baseModel;
            baseModel.Init(GetSupportedLocales(), GetSupportedTimeZones());
            var selectedLocale = baseModel.SupportedLocales.Where(l => String.Equals(l.Value, tmpModel.Locale, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (selectedLocale != null)
                selectedLocale.Selected = true;
            return baseModel;
        }

        [MonsterAuthorize(Constants.RoleUser, Constants.RoleAdmin)]
        public ActionResult ChangePassword()
        {
            return RedirectToAction("Edit");
        }

        [MonsterAuthorize(Constants.RoleUser, Constants.RoleAdmin)]
        [AcceptVerbs(HttpVerbs.Post)]
        [MonsterValidateAntiForgeryToken(Salt = Constants.Salt_Account_Edit)]
        [MenuActivator(MenuActivator.ActivationTypeEnum.EditProfile)]
        [ValidateInput(false)]
        public ActionResult ChangePassword(ProfilePasswordModel passwordModel)
        {
            Identity identity = User.Identity;

            if (ModelState.IsValid)
            {
                try
                {
                    _membershipService.ChangePassword(identity.UserId, passwordModel.NewPassword, passwordModel.OldPassword, DateTime.FromBinary(passwordModel.Stamp));
                    TempData.AddRequestSuccessMessage(SuccessMessagesResources.PasswordChangedInfo);
                    return RedirectToAction("Edit");
                }
                catch (StaleUserException)
                {
                    ModelState.AddModelError("profile", ValidationResources.AccountStalled);
                }
                catch (UserNotExistsException)
                {
                    ModelState.AddModelError("profile", ValidationResources.AccountNotFound);
                }
                catch (PasswordsMismatchException)
                {
                    ModelState.AddModelError("profile", ValidationResources.OldPasswordsMismatchError);
                }
                catch (InvalidPasswordException)
                {
                    ModelState.AddModelError("NewPassword", ValidationResources.RegInvalidPassword);
                }
            }

            //smth went wrong
            ProfileBaseModel baseModel = null;
            ProfileModel model = new ProfileModel
            {
                BaseModel = InitBaseProfileModel(ref baseModel, _userService.Load(identity.UserId)),
                PasswordModel = InitProfilePasswordModel(ref passwordModel, null)
            };
            var resultAction = Condition()
                .DoIfNotAjax(() => View("Edit", model))
                .DoIfAjax(() => Json(new AjaxResult
                {
                    MainPanelHtml = this.RenderViewToString("~/Views/Account/Controls/ProfileControl.ascx", model)
                }, JsonRequestBehavior.AllowGet));
            return resultAction;
        }

        [MonsterAuthorize(Constants.RoleUser, Constants.RoleAdmin)]
        [MenuActivator(MenuActivator.ActivationTypeEnum.EditProfile)]
        public ActionResult Edit()
        {
#warning return nav bar page
            ProfileModel model = null;
            var resultAction = Condition()
                        .DoIfNotAjax(() => View(model))
                        .DoIfAjax(() => Json(new AjaxResult
                        {
                            MainPanelHtml = this.RenderViewToString("~/Views/Account/Controls/ProfileControl.ascx", model)
                        }, JsonRequestBehavior.AllowGet));

            User user = _userService.Load(User.Identity.UserId);

            ProfileBaseModel baseModel = null;
            ProfilePasswordModel passwdModel = null;
            model = new ProfileModel()
            {
                BaseModel = InitBaseProfileModel(ref baseModel, user),
                PasswordModel = InitProfilePasswordModel(ref passwdModel, user)
            };
            return resultAction;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [MonsterValidateAntiForgeryToken(Salt = Constants.Salt_Account_Edit)]        
        [MenuActivator(MenuActivator.ActivationTypeEnum.EditProfile)]
        [ValidateInput(false)]
        public ActionResult Edit(ProfileBaseModel baseModel)
        {
            if (ModelState.IsValid)
            {
                Identity identity = User.Identity;

                try
                {
                    _membershipService.UpdateUser(identity.UserId, baseModel.UserName, baseModel.Locale, baseModel.TimeZone, DateTime.FromBinary(baseModel.Stamp), identity);
                    TempData.AddRequestSuccessMessage(SuccessMessagesResources.ProfileUpdateSuccessMessage);
                    return RedirectToAction("Edit");
                }
                catch (StaleUserException)
                {
                    ModelState.AddModelError("profile", ValidationResources.AccountStalled);
                }
                catch (InvalidUserNameException)
                {
                    ModelState.AddModelError("UserName", ValidationResources.RegInvalidUserName);
                }
                catch (UserNotExistsException)
                {
                    ModelState.AddModelError("profile", ValidationResources.AccountNotFound);
                }
            }

            ProfilePasswordModel passwdModel = null;
            baseModel = InitBaseProfileModel(ref baseModel, null);
            ProfileModel model = new ProfileModel
            {
                BaseModel = baseModel,
                PasswordModel = InitProfilePasswordModel(ref passwdModel, null)
            };
            var resultAction = Condition()
                .DoIfNotAjax(() => View(model))
                .DoIfAjax(() => Json(new AjaxResult
                {
                    MainPanelHtml = this.RenderViewToString("~/Views/Account/Controls/ProfileControl.ascx", model)
                }, JsonRequestBehavior.AllowGet));
            return resultAction;
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

			// Attempt to register the user
			MembershipCreateStatus createStatus = _membershipService.CreateUser(model.Email, model.Password, model.UserName, model.Locale, model.TimeZone);

			if (createStatus == MembershipCreateStatus.Success)
			{
				_formsAuthService.SignIn(model.Email, false /* createPersistentCookie */);
				return RedirectToAction("Index", "Home");
			}
			
            ModelState.AddModelError("_FORM", ErrorCodeToString(createStatus));

            model.Init(GetSupportedLocales(), GetSupportedTimeZones(), _membershipService.MinPasswordLength);
			// If we got this far, something failed, redisplay form
			return View(model);
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
                            Html = this.RenderViewToString("~/Views/Account/Controls/LogOnFormControl.ascx", model)
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

			if (Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

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
