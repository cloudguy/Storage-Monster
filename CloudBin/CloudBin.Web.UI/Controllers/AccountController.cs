﻿using System.Web.Mvc;
using CloudBin.Web.Core.Security;
using CloudBin.Web.UI.Models.Account;

namespace CloudBin.Web.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult SignOut()
        {
            _authenticationService.SignOut();
            return RedirectToAction("Index", "Main");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [ValidateInput(false)]
        public ActionResult Register()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Main");
            }

            return View("Register", new RegisterModel());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Register(RegisterModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Main");
            }

            return View("Register", model);
        }
    }
}