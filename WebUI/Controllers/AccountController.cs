using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entities;
using Domain.Abstract;
using Domain.Concrete;
using WebUI.Models;
using WebUI.Infrastructure.Abstract;
using WebUI.Infrastructure.Concrete;
namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private IAuthenticateProvider Authentication;
        public AccountController(IAuthenticateProvider Auth)
        {
            Authentication = Auth;
        }

        public ViewResult BootIndex()
        {
            return View();
        }

        //
        // GET: /LogIn/
        
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(LoginViewModel logInVM)
        {
            bool isAuthenticate = Authentication.LogIn(logInVM);
            if (isAuthenticate == false)
            {
                ModelState.AddModelError("", "Nieprawidłowy login lub hasło");
                return View();
            }
                return RedirectToAction("Index", "Home");

        }
        [Authorize(Roles = "Admin, Biuro, Klient")]
        public RedirectToRouteResult LogOut()
        {
            Authentication.LogOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin, Biuro, Klient")]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [Authorize(Roles = "Admin, Biuro, Klient")]
        [HttpPost]
        public ActionResult ChangePassword(PasswordChangeViewModel passwordVM)
        {
            if (Authentication.ChangePassword(passwordVM))
            {
                return RedirectToAction("Home", "Index");
            }
            ModelState.AddModelError("", "błędna nazwa użytkownika lub hasło");
            return View();
            
        }
    }
}
