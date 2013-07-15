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
        public UsersController(IUserRepository repository, IUserRolesRepository userRolesRepository)
        {
            this.repo = repository;
            this.userRolesRepo = userRolesRepository;
        }
        //
        // GET: /Users/

        public ActionResult Index()
        {
            List<User> users = repo.Users.ToList();
            return View();
        }

        public ActionResult Create()
        {
            
            
            return View();
        }
        [HttpPost]
        public ActionResult Create(UserViewModel userViewModel, User user, UserRole uR)
        {

            userViewModel.user.UserRole = new EFUserRoleRepository().UsersRoles.FirstOrDefault(u => u.RoleName == "Customer");
            if (userViewModel.user.UserRole != null)
            {
                ModelState["RoleName"].Errors.Clear();
                 ModelState["user.UserRole"].Errors.Clear();
                
            }
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

        public ActionResult Edit()
        {
            return View();
        }

    }
}
