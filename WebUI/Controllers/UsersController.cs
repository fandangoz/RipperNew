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
using WebUI.Helpers;
namespace WebUI.Controllers
{
    public class UsersController : Controller
    {
        public const int PageSize = 10;
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
        public ViewResult BootIndex() {
            return View();
        }
         [Authorize(Roles = "Admin, Biuro")]
        public ViewResult Index()
        {
            return View();
        }
         [Authorize(Roles = "Admin, Biuro")]
        public PartialViewResult UsersList(int page = 1, string name = "", string surname = "", string login = "", string selectedRole = "", bool? showInactiveUsers = false, int companyID = 0)
        {
            
            var pagedData = new PagedData<User>();
            var users = repo.Users;
            if (showInactiveUsers == false)
            {
                users = from u in users where u.isActive == true select u;
            }
            if (companyID != 0)
            {
                users = from u in users where u.Company.CompanyID == companyID select u;
            }
            var userRoles = userRolesRepo.UsersRoles;
            foreach (UserRole uR in userRoles)
            {
                if (selectedRole.Equals(uR.RoleName))
                {
                    users = from u in users where u.UserRole.RoleName.Equals(selectedRole) select u;
                    break;
                }
            }
            if (login.Length > 0)
            {
                users = from u in users where u.Login.ToLower().StartsWith(login.ToLower()) select u;
            }
            if (surname.Length > 0)
            {
                users = from u in users where u.Surname.ToLower().StartsWith(surname.ToLower()) select u;
            }
            if (name.Length > 0)
            {
                users = from u in users where u.Name.ToLower().StartsWith(name.ToLower()) select u;
            }
            pagedData.Data = users.OrderBy(u => u.Login).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
            pagedData.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)users.Count() / PageSize));
            pagedData.CurrentPage = page;
            return PartialView(pagedData);
        }
         [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Create() 
        { 
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Create(UserViewModel userViewModel)   
        {
            userViewModel.user.isActive = true;
            userViewModel.user.UserRole = userRolesRepo.UsersRoles.FirstOrDefault(u => u.UserRoleID == 2);
            if (userViewModel.user.UserRole != null)
            {
                if(ModelState["user.UserRole"] != null)
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
            if (userViewModel.Password.Length < 4)
            {
                ModelState.AddModelError("", "Pola hasło musi posiadać przynajmniej 4 znaki");
            }
            if (userViewModel.Password != userViewModel.passwordConfirmation)
            {
                ModelState.AddModelError("", "Pola hasło i potwierdzenie hasła muszą być jednakowe");
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
         [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Edit(int id=0)
        {
            User user = repo.Users.FirstOrDefault(u => u.UserID == id);
            
            if (user == null)
            {
                return HttpNotFound();
            }
            var rolesList = userRolesRepo.UsersRoles.Select(r => new SelectListItem { Value = r.RoleName, Text = r.RoleName }).ToList();
            SelectList RoleSelectListTmp = new SelectList(rolesList, "Value", "Text");
            return View(new UserViewModel { user = user, CompanyName = user.Company != null ? user.Company.CompanyName : null,
                        RoleSelectList = RoleSelectListTmp, userRoleName = user.UserRole.RoleName });
        
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Edit(UserViewModel userMV)
        {
            User user = userMV.user;
            User oldUser = repo.Users.FirstOrDefault(u => u.UserID == user.UserID);
            user.UserRole = userRolesRepo.UsersRoles.FirstOrDefault(uR => uR.RoleName.Equals(userMV.userRoleName));
            user.Password = oldUser.Password;
            user.PasswordSalt = oldUser.PasswordSalt;
            if (!user.UserRole.RoleName.Equals("Klient") && !User.IsInRole("Admin"))
            {
                ModelState.AddModelError("","Nie masz wystarczających uprawnień aby edytować tego użytkownika, skontaktuj się z administratorem");
            }
            if (userMV.CompanyName != null)
            {
                user.Company = companiesRepo.Companies.FirstOrDefault(c => c.CompanyName.Equals(userMV.CompanyName));
                if (user.Company == null)
                {
                    ModelState.AddModelError("", "Podana firma nie znajduje się w bazie danych");
                }
            }
            if (userMV.userRoleName != "Klient" && userMV.CompanyName != null)
            {
                ModelState.AddModelError("", "Tylko klient może mieć przypisaną firmę");
            }
            if (user.UserRole != null)
            {
                if (ModelState["user.userRole"] != null)
                    ModelState["user.userRole"].Errors.Clear();
            }
            if (user.Password != null && ModelState["passwordConfirmation"] != null)
                ModelState["passwordConfirmation"].Errors.Clear();
            if (user.PasswordSalt != null && ModelState["password"] != null)
                ModelState["password"].Errors.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    repo.EditUser(user);
                }
                catch (UserExistInDatabaseException ex)
                {
                    ModelState.AddModelError("", ex);
                }
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = user.UserID });
                }
            }
            var rolesList = userRolesRepo.UsersRoles.Select(r => new SelectListItem { Value = r.RoleName, Text = r.RoleName });
            userMV.RoleSelectList = new SelectList(rolesList, "Value", "Text");
            userMV.userRoleName = user.UserRole.RoleName;
            return View(userMV);
        }
        //public ActionResult ChangeRole(int id = 0)
        //{
        //    User user = repo.Users.FirstOrDefault(u => u.UserID == id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var rolesList = userRolesRepo.UsersRoles.Select(r => new SelectListItem { Value = r.RoleName, Text = r.RoleName }).ToList();
        //    SelectList RoleSelectListTmp = new SelectList(rolesList, "Value", "Text");
        //    return View(new UserViewModel { user = user, RoleSelectList = RoleSelectListTmp, userRoleName = user.UserRole.RoleName });
        //}
        //[HttpPost]
        //public ActionResult ChangeRole(UserViewModel uVM)
        //{
        //        try
        //        { 
        //            repo.ChangeUserRole(uVM.user, uVM.userRoleName);
        //            return RedirectToAction("Details", new { id = uVM.user.UserID });
        //        }
        //        catch(_ItemNotExistInDatabaseException  ex)
        //        {
        //            ModelState.AddModelError("", ex);
        //        }
        //        User user = repo.Users.FirstOrDefault(u => u.UserID == uVM.user.UserID);
        //        var rolesList = userRolesRepo.UsersRoles.Select(r => new SelectListItem { Value = r.RoleName, Text = r.RoleName });
        //        SelectList RoleSelectListTmp = new SelectList(rolesList, "Value", "Text");
        //    return View(new UserViewModel { user = user, RoleSelectList = RoleSelectListTmp, userRoleName = user.UserRole.RoleName });
        //}
         [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Details(int id = 0)
        {
            User user = repo.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        
    }
}
