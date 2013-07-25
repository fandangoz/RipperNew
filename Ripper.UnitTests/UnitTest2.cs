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
namespace Ripper.UnitTests
{
    [TestClass]
    public class CompanyTest
    {
        [TestMethod]
        public void AddCompany()
        {
            Mock<ICompaniesRepository> mock2 = new Mock<ICompaniesRepository>();
            //mock2.Setup(m => m.Companies).Returns( new Company[] {
            //    new Company { CompanyID= 0, CompanyName = "C1"},
            //    new Company { CompanyID= 1, CompanyName = "C2" }
            //}.AsQueryable());
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            CompanyController target = new CompanyController(mock2.Object, mock.Object);
            Company c = new Company { CompanyName = "Firma1" };
            ActionResult result = target.Create(c);
            mock2.Verify(m => m.SaveCompany(c));
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
        [TestMethod]
        public void Cannot_Save_Invalid_Company()
        {
            Mock<ICompaniesRepository> mock2 = new Mock<ICompaniesRepository>();
            //mock2.Setup(m => m.Companies).Returns( new Company[] {
            //    new Company { CompanyID= 0, CompanyName = "C1"},
            //    new Company { CompanyID= 1, CompanyName = "C2" }
            //}.AsQueryable());
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            CompanyController target = new CompanyController(mock2.Object, mock.Object);
            Company c = new Company();
            target.ModelState.AddModelError("", "error");
            ActionResult result = target.Create(c);
            mock2.Verify(m => m.SaveCompany(c), Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public void Can_Edit_Valid_Company()
        {
            Mock<ICompaniesRepository> mock2 = new Mock<ICompaniesRepository>();
            mock2.Setup(m => m.Companies).Returns( new Company[] {
               new Company { CompanyID= 1, CompanyName = "C1"},
                new Company { CompanyID= 2, CompanyName = "C2" }
            }.AsQueryable());
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            CompanyController target = new CompanyController(mock2.Object, mock.Object);
            Company c1 = target.Edit(1).ViewData.Model as Company;
            Company c2 = target.Edit(2).ViewData.Model as Company;
            Assert.AreEqual(1, c1.CompanyID);
            Assert.AreEqual(2, c2.CompanyID);
        }
        [TestMethod]
        public void Cannot_Edit_Invalid_Company()
        {
            Mock<ICompaniesRepository> mock2 = new Mock<ICompaniesRepository>();
            mock2.Setup(m => m.Companies).Returns(new Company[] {
               new Company { CompanyID= 1, CompanyName = "C1"},
                new Company { CompanyID= 2, CompanyName = "C2" }
            }.AsQueryable());
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            CompanyController target = new CompanyController(mock2.Object, mock.Object);
            Company c1 = target.Edit(4).ViewData.Model as Company;
            Assert.IsNull(c1);
        }
        [TestMethod]
        public void Company_Controller_Index_Test()
        {
            Mock<ICompaniesRepository> mock2 = new Mock<ICompaniesRepository>();
            mock2.Setup(m => m.Companies).Returns( new Company[] {
                new Company { CompanyID= 0, CompanyName = "C1"},
                new Company { CompanyID= 1, CompanyName = "C2" },
                new Company { CompanyID= 2, CompanyName = "C3" },
                new Company { CompanyID= 3, CompanyName = "C4" }

            }.AsQueryable());
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            CompanyController target = new CompanyController(mock2.Object, mock.Object);
            PagedData<Company> result = (PagedData<Company>) target.CompaniesList().ViewData.Model;
            Assert.AreEqual(4, result.Data.Count());
            target = new CompanyController(mock2.Object, mock.Object);
            result = (PagedData<Company>)target.CompaniesList(companyName:"C2" ).ViewData.Model;
            Assert.AreEqual(1, result.Data.Count());
            Assert.AreEqual("C2", result.Data.First().CompanyName);

        }

        [TestMethod]
        public void AutoCompleteTest()
        {
            Mock<ICompaniesRepository> mock2 = new Mock<ICompaniesRepository>();
            mock2.Setup(m => m.Companies).Returns(new Company[] {
                new Company { CompanyID= 0, CompanyName = "C1"},
                new Company { CompanyID= 1, CompanyName = "C2" },
                new Company { CompanyID= 2, CompanyName = "C3" },
                new Company { CompanyID= 3, CompanyName = "C4" }

            }.AsQueryable());
            Mock<IUserRepository> mock = new Mock<IUserRepository>();
            CompanyController target = new CompanyController(mock2.Object, mock.Object);
            JsonResult actual = target.AutoCompleteCompanyName("C1") as JsonResult;
            var data = actual.Data;
            var type = data.GetType();
            var countPropretyInfo = type.GetProperty("Count");
            var expectedCount = countPropretyInfo.GetValue(data, null);
            Assert.AreEqual(1, expectedCount);
            actual = target.AutoCompleteCompanyName("c") as JsonResult;
            data = actual.Data;
            expectedCount = countPropretyInfo.GetValue(data, null);
            Assert.AreEqual(4, expectedCount);
        }
    }
}
