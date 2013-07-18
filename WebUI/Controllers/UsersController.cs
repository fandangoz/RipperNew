using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Concrete;
using Domain.Entities;
using Domain.Utilities;
using Domain.Abstract;
using WebUI.Models;
namespace WebUI.Controllers
{
    public class UsersController : Controller
    {
        private IUserRepository repo;
        private IUserRolesRepository userRolesRepo;
        private ICompaniesRepository companiesRepo;
        public UsersController(IUserRepository repository, IUserRolesRepository userRolesRepository, ICompaniesRepository compRepo)
        {
            this.repo = repository;
            this.userRolesRepo = userRolesRepository;
            this.companiesRepo = compRepo;
        }
        //
        // GET: /Users/

        public ActionResult Index()
        {
            return View(repo.Users);
        }

        public ActionResult Create() 
        {
            
            
            return View();
        }
        [HttpPost]
        public ActionResult Create(UserViewModel userViewModel)
        {
            userViewModel.user.UserRole = userRolesRepo.UsersRoles.FirstOrDefault(u => u.UserRoleID == 2);
            if (userViewModel.user.UserRole != null)
            {
                 ModelState["user.UserRole"].Errors.Clear();               
            }
            if (userViewModel.CompanyName != null)
            {
                Company userCompany = companiesRepo.Companies.FirstOrDefault(c => c.CompanyName == userViewModel.CompanyName);
                if (userCompany == null)
                {
                    ModelState.AddModelError("", "Nie znaleziono podanej firmy w bazie danych popraw nazwę firmy lub dodaj ją do bazy danych");
                }
                userViewModel.user.Company = companiesRepo.Companies.FirstOrDefault(c => c.CompanyName == userViewModel.CompanyName);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    repo.SaveUser(userViewModel.user);
                }
                catch (UserExistInDatabaseException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index","Users");
                }
            }
            return View(userViewModel);
        }

        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult Edit(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    repo.SaveUser(userViewModel.user);
                }
                catch (UserExistInDatabaseException ex)
                {
                    ModelState.AddModelError("", ex);
                }
                if (ModelState.IsValid)
                {
                    RedirectToRoute("Index");
                }
            }
            return View(userViewModel);
        }

    }
}
