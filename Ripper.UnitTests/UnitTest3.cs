using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using WebUI.Infrastructure.Abstract;
using WebUI.Infrastructure.Concrete;
using Moq;
using Domain.Abstract;
using System.Collections.Generic;
using System.Linq;
using WebUI.Controllers;
using WebUI.Models;
using System.Web.Mvc;
namespace Ripper.UnitTests
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void LogInPassTest()
        {
            LoginViewModel lvm = new LoginViewModel { Login = "login", Password = "1234567" };
            Mock<IAuthenticateProvider> mock = new Mock<IAuthenticateProvider>();
            mock.Setup(m => m.LogIn(lvm)).Returns(true);
            AccountController aController = new AccountController(mock.Object);
            ActionResult result =  aController.LogIn(lvm);
            Assert.IsInstanceOfType(result , typeof(RedirectToRouteResult));            
        }
        [TestMethod]
        public void LogInFailTest()
        {
            LoginViewModel lvm = new LoginViewModel { Login = "login", Password = "1234567" };
            Mock<IAuthenticateProvider> mock = new Mock<IAuthenticateProvider>();
            mock.Setup(m => m.LogIn(lvm)).Returns(false);
            AccountController aController = new AccountController(mock.Object);
            ActionResult result = aController.LogIn(lvm);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

    }
}
