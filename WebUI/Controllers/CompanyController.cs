using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using Domain.Utilities;
using WebUI.Helpers;
using WebUI.Models;
namespace WebUI.Controllers
{
    public class CompanyController : Controller
    {
        const int PageSize = 10;
        private ICompaniesRepository CompaniesRepo { set; get; }
        private IUserRepository UsersRepo { set; get; }
        public CompanyController(ICompaniesRepository repo, IUserRepository Urepo)
        {
            CompaniesRepo = repo;
            UsersRepo = Urepo;
        }

        //
        // GET: /Company/

        public ViewResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Biuro")]
        public PartialViewResult CompaniesList(int page = 1, string companyName = "", string companyRegon = "")
        {

            var pagedData = new PagedData<Company>();
            var companies = CompaniesRepo.Companies;
            if (companyName.Length > 0)
            {
                companies = from c in companies where c.CompanyName.ToLower().StartsWith(companyName.ToLower()) select c;
            }
            if (companyRegon.Length > 0)
            {
                companies = from c in companies where c.CompanyRegon.ToLower().StartsWith(companyRegon.ToLower()) select c;
            }
            pagedData.Data = companies.OrderBy(c => c.CompanyName).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
            pagedData.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)companies.Count() / PageSize));
            pagedData.CurrentPage = page;
            return PartialView(pagedData);
        }

         [Authorize(Roles = "Admin, Biuro")]
        public ViewResult Create()
        {
            return View();
        }
         [Authorize(Roles = "Admin, Biuro")]
        public ViewResult Edit(int id)
        {
                Company company = CompaniesRepo.Companies.FirstOrDefault(c => c.CompanyID == id);
                return View(company);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Edit(Company company)
        {
            return Create(company);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Create(Company company)
        {
            if (!ModelState.IsValid)
            {
                return View(company);
            }
            try
            {
                CompaniesRepo.SaveCompany(company);
                return RedirectToAction("Index", "Company");
            }
            catch (UserExistInDatabaseException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(company);
            }           
        }
        
        public PartialViewResult _AddNewCompany()
        {
            return PartialView();
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Biuro")]
        public JsonResult _addNewCompany(Company company)
        {
            string output = "";
            if (ModelState.IsValid)
            {
                try
                {
                    CompaniesRepo.SaveCompany(company);
                }
                catch (UserExistInDatabaseException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                output = "Firma " + company.CompanyName + " została poprawnie dodana";
            }
            if(!ModelState.IsValid)
            {
                output = "";
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        output += error.ErrorMessage;
                    }
                }
            }
            return Json(output, JsonRequestBehavior.AllowGet);
        }
         [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Details(int id = 0)
        {
            Company company = CompaniesRepo.Companies.FirstOrDefault(c => c.CompanyID == id);
            if (company == null)
            {
                return HttpNotFound();
            }
            var users = from u in UsersRepo.Users where u.Company.CompanyID == company.CompanyID select u;
            return View(new CompanyUsersViewModel { Company = company, Users = users.ToArray() });
        }
        public JsonResult AutoCompleteCompanyName(string term)
        {
            var result = (from c in CompaniesRepo.Companies
                          where c.CompanyName.ToLower().Contains(term.ToLower())
                          select new { c.CompanyName }).Distinct().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ViewResult BootIndex()
        {
            return View();
        }

    }
}
