using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace WebUI.Models
{
    public class UserViewModel
    {
        public User user { set; get; }
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage="hasla nie są jednakowe")]
        [Display(Name = "Potwierdz hasło: ")]
        public string passwordConfirmation { set; get; }
        public string Password { get { return user.Password; } }
    }
}