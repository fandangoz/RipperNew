using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Infrastructure.Abstract;
using Domain.Concrete;
using Domain.Entities;
using Domain.Abstract;
using WebUI.Models;
using SimpleCrypto;
using System.Web.Security;
using WebUI.Infrastructure;
namespace WebUI.Infrastructure.Concrete
{
    public class AuthenticateProvider : IAuthenticateProvider
    {
        private IUserRepository repo;
        public AuthenticateProvider(IUserRepository Urepo)
        {
            repo = Urepo;
        }

        public bool LogIn(LoginViewModel logInVM)
        {
            User user = repo.IsValid(logInVM.Login,logInVM.Password);
            
            if(user != null)
            {
                FormsAuthentication.SetAuthCookie(logInVM.Login, false);
                return true;
            }
            return false;
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        public bool ChangePassword(PasswordChangeViewModel passwordVM)
        {
            string username = HttpContext.Current.User.Identity.Name;
            if (repo.ChangeUserPassword(username, passwordVM.OldPassword, passwordVM.NewPassword) != null)
            {
                return true;
            }
            return false;
        }


    }
}