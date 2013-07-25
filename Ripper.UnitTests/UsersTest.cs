using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using Domain.Concrete;
using Moq;
using Domain.Abstract;
using System.Collections.Generic;
using System.Linq;
using WebUI.Controllers;
using WebUI.Models;
using System.Web.Mvc;
using WebUI.Helpers;
namespace Ripper.UnitTests.DomainTests
{
    [TestClass]
    public class UsersTest
    {
        [TestMethod]
        public void Create_Valid_User_Test()
        {
            var crypto = new SimpleCrypto.PBKDF2();
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            mock.Setup(m => m.Users).Returns(new User[] {
                new User { UserID = 1 , Login = "Remek@gmail.com", Name = "Remek", Surname="Zieliński", Address="Różana 5/1 Szczecin", Password = crypto.Compute("123456"),
                    PasswordSalt = crypto.Salt, Phone = "12343", additionalInformation="brak", UserRole = new UserRole { UserRoleID=0, RoleName = "Customer" } , 
                    Company = new Company { CompanyID = 0, CompanyName="Company1" } },
                    new User { UserID = 2 , Login = "Remek1@gmail.com", Name = "Remek1", Surname="Zieliński1", Address="Różana 5/1 Szczecin1", Password = crypto.Compute("1234567"),
                    PasswordSalt = crypto.Salt, Phone = "123439", additionalInformation="brak1", UserRole = new UserRole { UserRoleID=0, RoleName = "Customer" } , 
                    Company = new Company { CompanyID = 0, CompanyName="Company1" } },
                    new User { UserID = 3 , Login = "Remek2@gmail.com", Name = "Remek2", Surname="Zieliński2", Address="Różana 5/1 Szczecin2", Password = crypto.Compute("1234562"),
                    PasswordSalt = crypto.Salt, Phone = "123431", additionalInformation="brak2", UserRole = new UserRole { UserRoleID=1, RoleName = "Admin" } , 
                    Company = new Company { CompanyID = 1, CompanyName="Company2" } },
            }.AsQueryable());
            Mock<IUserRolesRepository> mock1 = new Mock<IUserRolesRepository>();
            mock1.Setup(m => m.UsersRoles).Returns(new UserRole[]
            {
                new UserRole { UserRoleID=2, RoleName = "Customer" },
                new UserRole { UserRoleID=1, RoleName = "Admin" }

            }.AsQueryable());
            Mock<ICompaniesRepository> mock2 = new Mock<ICompaniesRepository>();
            mock2.Setup(m => m.Companies).Returns(new Company[] {
                new Company { CompanyID= 1, CompanyName = "C1"},
                new Company { CompanyID= 2, CompanyName = "C2" }
            }.AsQueryable());
            UsersController uController = new UsersController(mock.Object, mock1.Object,mock2.Object);

            UserViewModel uVM = new UserViewModel
            {
                user = new User
                {
                    Login = "Remek@gmail.com",
                    Password = "123456",
                    UserRole = new UserRole { UserRoleID = 2, RoleName = "Customer" }
                },
                passwordConfirmation = "123456"
            };
            ActionResult result = uController.Create(uVM);
            mock.Verify(m => m.SaveUser(uVM.user));
            uVM.user.UserRole = null;
            result = uController.Create(uVM);
            mock.Verify(m => m.SaveUser(uVM.user));
            uVM.CompanyName = "C1";
            result = uController.Create(uVM);
            mock.Verify(m => m.SaveUser(uVM.user));      
        }
        [TestMethod]
        public void Cannot_Create_InValid_User_Test()
        {
            var crypto = new SimpleCrypto.PBKDF2();
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            mock.Setup(m => m.Users).Returns(new User[] {
                new User { UserID = 1 , Login = "Remek@gmail.com", Name = "Remek", Surname="Zieliński", Address="Różana 5/1 Szczecin", Password = crypto.Compute("123456"),
                    PasswordSalt = crypto.Salt, Phone = "12343", additionalInformation="brak", UserRole = new UserRole { UserRoleID=0, RoleName = "Customer" } , 
                    Company = new Company { CompanyID = 0, CompanyName="Company1" } },
                    new User { UserID = 2 , Login = "Remek1@gmail.com", Name = "Remek1", Surname="Zieliński1", Address="Różana 5/1 Szczecin1", Password = crypto.Compute("1234567"),
                    PasswordSalt = crypto.Salt, Phone = "123439", additionalInformation="brak1", UserRole = new UserRole { UserRoleID=0, RoleName = "Customer" } , 
                    Company = new Company { CompanyID = 0, CompanyName="Company1" } },
                    new User { UserID = 3 , Login = "Remek2@gmail.com", Name = "Remek2", Surname="Zieliński2", Address="Różana 5/1 Szczecin2", Password = crypto.Compute("1234562"),
                    PasswordSalt = crypto.Salt, Phone = "123431", additionalInformation="brak2", UserRole = new UserRole { UserRoleID=1, RoleName = "Admin" } , 
                    Company = new Company { CompanyID = 1, CompanyName="Company2" } },
            }.AsQueryable());
            Mock<IUserRolesRepository> mock1 = new Mock<IUserRolesRepository>();
            mock1.Setup(m => m.UsersRoles).Returns(new UserRole[]
            {
                new UserRole { UserRoleID=2, RoleName = "Customer" },
                new UserRole { UserRoleID=1, RoleName = "Admin" }

            }.AsQueryable());
            Mock<ICompaniesRepository> mock2 = new Mock<ICompaniesRepository>();
            mock2.Setup(m => m.Companies).Returns(new Company[] {
                new Company { CompanyID= 1, CompanyName = "C1"},
                new Company { CompanyID= 2, CompanyName = "C2" }
            }.AsQueryable());
            UsersController uController = new UsersController(mock.Object, mock1.Object, mock2.Object);

            UserViewModel uVM = new UserViewModel
            {
                user = new User
                {
                    Login = "Rem",
                    Password = "123456",
                },
                passwordConfirmation = "1236",
                CompanyName = "sss"
            };

            //uController.ModelState.AddModelError("", "");
            ActionResult result = uController.Create(uVM);
            mock.Verify(m => m.SaveUser(uVM.user), Times.Never());
            uVM.CompanyName = "";
            uController.ModelState.AddModelError("", "");
            result = uController.Create(uVM);
            mock.Verify(m => m.SaveUser(uVM.user), Times.Never()); 
        }
        [TestMethod]
        public void UserList_Test_All()
        {

            UsersController uController = GetUserController(); 
            
            PagedData<User> pagedData = (PagedData<User>)uController.UsersList().ViewData.Model;
            IEnumerable<User> users = pagedData.Data;
            Assert.AreEqual(2, users.Count());
            pagedData = (PagedData<User>)uController.UsersList(showInactiveUsers: true).ViewData.Model;
            users = pagedData.Data;
            Assert.AreEqual(3, users.Count());
        }
        [TestMethod]
        public void UserList_Test_Parameters_Filter()
        {

            UsersController uController = GetUserController();

            PagedData<User> pagedData = (PagedData<User>)uController.UsersList().ViewData.Model;

            pagedData = (PagedData<User>)uController.UsersList(name: "remek1").ViewData.Model;
            IEnumerable<User> users = pagedData.Data;
            Assert.AreEqual("Remek1", users.First().Name);
            Assert.AreEqual(1, users.Count());
            pagedData = (PagedData<User>)uController.UsersList(surname: "Zieliński1").ViewData.Model;
            users = pagedData.Data;
            Assert.AreEqual("Zieliński1", users.First().Surname);
            Assert.AreEqual(1, users.Count());
            pagedData = (PagedData<User>)uController.UsersList(login: "Remek1@gmail.com").ViewData.Model;
            users = pagedData.Data;
            Assert.AreEqual("Remek1@gmail.com", users.First().Login);
            Assert.AreEqual(1, users.Count());
            pagedData = (PagedData<User>)uController.UsersList(selectedRole: "Customer").ViewData.Model;
            users = pagedData.Data;
            Assert.AreEqual(1, users.Count());
            Assert.AreEqual("Remek1@gmail.com", users.First().Login);


        }

        private static UsersController GetUserController()
        {
            Mock<IUserRepository> mock;
            Mock<IUserRolesRepository> mock1;
            Mock<ICompaniesRepository> mock2;
            var crypto = new SimpleCrypto.PBKDF2();
            mock = new Mock<IUserRepository>();
            mock.Setup(m => m.Users).Returns(new User[] {
                new User { UserID = 1 , Login = "Remek@gmail.com", Name = "Remek", Surname="Zieliński", Address="Różana 5/1 Szczecin", Password = crypto.Compute("123456"),
                    PasswordSalt = crypto.Salt, Phone = "12343", additionalInformation="brak", UserRole = new UserRole { UserRoleID=11, RoleName = "Office" } , 
                    Company = new Company { CompanyID = 0, CompanyName="Company1" }, isActive = true },
                    new User { UserID = 2 , Login = "Remek1@gmail.com", Name = "Remek1", Surname="Zieliński1", Address="Różana 5/1 Szczecin1", Password = crypto.Compute("1234567"),
                    PasswordSalt = crypto.Salt, Phone = "123439", additionalInformation="brak1", UserRole = new UserRole { UserRoleID=2, RoleName = "Customer" } , 
                    Company = new Company { CompanyID = 0, CompanyName="Company1" }, isActive = true },
                    new User { UserID = 3 , Login = "Remek2@gmail.com", Name = "Remek2", Surname="Zieliński2", Address="Różana 5/1 Szczecin2", Password = crypto.Compute("1234562"),
                    PasswordSalt = crypto.Salt, Phone = "123431", additionalInformation="brak2", UserRole = new UserRole { UserRoleID=1, RoleName = "Admin" } , 
                    Company = new Company { CompanyID = 1, CompanyName="Company2" } },
            }.AsQueryable());

            mock1 = new Mock<IUserRolesRepository>();
            mock1.Setup(m => m.UsersRoles).Returns(new UserRole[]
            {
                new UserRole { UserRoleID=2, RoleName = "Customer" },
                new UserRole { UserRoleID=1, RoleName = "Admin" }

            }.AsQueryable());
            mock2 = new Mock<ICompaniesRepository>();

            return new UsersController(mock.Object, mock1.Object, mock2.Object);
        }  
    }
}
