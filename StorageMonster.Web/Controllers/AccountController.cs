using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StorageMonster.Services;
using StorageMonster.Services.Security;
using StorageMonster.Web.Properties;
using StorageMonster.Web.Models.Accounts;
using StorageMonster.Common;

namespace StorageMonster.Web.Controllers
{
    public class AccountController : BaseController
    {
        protected IFormsAuthenticationService FormsAuthService;
        protected IMembershipService MembershipService;
        protected ILocaleProvider LocaleProvider;
		protected ICacheService CacheService;

        public AccountController(IFormsAuthenticationService formsAuthService, 
			IMembershipService membershipService, 
			ILocaleProvider localeProvider,
			ICacheService cacheService)
        {
            FormsAuthService = formsAuthService;
            MembershipService = membershipService;
            LocaleProvider = localeProvider;
			CacheService = cacheService;
        }
        
        public ActionResult LogOff()
        {
            FormsAuthService.SignOut();
            //Response.RemoveOutputCacheItem("/Home/Index");
            HttpCookie myCookie = new HttpCookie(FormsAuthentication.FormsCookieName)
                {
                    Expires = DateTime.Now.AddDays(-1d)
                };
            Response.Cookies.Add(myCookie);
            //Response.RemoveOutputCacheItem("/Home/Index");
            //Response
            return RedirectToAction("Index", "Home");
        }        

		protected IEnumerable<SelectListItem> GetSupportedLocales()
		{
			return CacheService.Get(Constants.LocaleDropDownListKeyCacheKey, () => 
                LocaleProvider.SupportedLocales.Select(x => new SelectListItem
			    {
			        Text = x.FullName,
			        Value = x.ShortName,
			        Selected = false
			    }).ToList() /*override lazy init*/);				
		}		

        public ActionResult Register()
        {
			RegisterModel model = new RegisterModel();
			model.Init(GetSupportedLocales(), MembershipService.MinPasswordLength);			
			return View(model);
        }

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Register(RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				if (model == null)
					model = new RegisterModel();
				model.Init(GetSupportedLocales(), MembershipService.MinPasswordLength);
				return View(model);
			}

			model.Init(GetSupportedLocales(), MembershipService.MinPasswordLength);

			// Attempt to register the user
			MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email, model.Locale);

			if (createStatus == MembershipCreateStatus.Success)
			{
				FormsAuthService.SignIn(model.Email, false /* createPersistentCookie */);
				return RedirectToAction("Index", "Home");
			}
			
            ModelState.AddModelError("_FORM", ErrorCodeToString(createStatus));

			// If we got this far, something failed, redisplay form
			return View(model);
		}


        public ActionResult LogOn()
        {
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
                return View(model);
            }


            if (!MembershipService.ValidateUser(model.Email, model.Password))
            {
                ModelState.AddModelError("_FORM", ValidationResources.UserNameOrPasswordIncorrect);
                return View(model);
            }

			FormsAuthService.SignIn(model.Email, model.RememberMe);
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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException("Windows authentication is not supported.");
            }
        }        
    }
}
