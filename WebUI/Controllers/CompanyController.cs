using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using Domain.Utilities;
namespace WebUI.Controllers
{
    public class CompanyController : Controller
    {
        private ICompaniesRepository CompaniesRepo { set; get; }
        public CompanyController(ICompaniesRepository repo)
        {
            CompaniesRepo = repo;
        }

        //
        // GET: /Company/

        public ActionResult Index()
        {
            return View(CompaniesRepo.Companies.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(int id = 0)
        {
            if (id != 0)
            {
                Company company = CompaniesRepo.Companies.FirstOrDefault(c => c.CompanyID == id);
                return View(company);
            }
            return RedirectToAction("Index", "company");
        }
        [HttpPost]
        public ActionResult Edit(Company company)
        {
            return Create(company);
        }
        [HttpPost]
        public ActionResult Create(Company company)
        {
            if (!ModelState.IsValid)
            {
                return View(company);
            }
            try
            {
                CompaniesRepo.SaveCompany(company);
                string refeee = Request.UrlReferrer.ToString();
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
        public JsonResult AutoCompleteCompanyName(string term)
        {
            var result = (from c in CompaniesRepo.Companies
                          where c.CompanyName.ToLower().Contains(term.ToLower())
                          select new { c.CompanyName }).Distinct().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
