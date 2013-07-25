using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace WebUI.Models
{
    public class UserViewModel
    {
        public User user { set; get; }
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "hasla nie są jednakowe")]
        [Display(Name = "Potwierdz hasło: ")]
        public string passwordConfirmation { set; get; }
        public string Password { get { return user.Password; } }
        public string CompanyName { set; get; }
        public string userRoleName { set; get; }
        public SelectList RoleSelectList { set; get; }

    }
}