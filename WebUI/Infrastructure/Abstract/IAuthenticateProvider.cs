using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUI.Models;
namespace WebUI.Infrastructure.Abstract
{
    public interface IAuthenticateProvider
    {
        bool LogIn(LoginViewModel logInVM);
        void LogOut();
        bool ChangePassword(PasswordChangeViewModel passwordVM);
    }
}
