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
        [Required(AllowEmptyStrings = false, ErrorMessage = "Potwierdzenie hasła jest wymagane")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "pole - potwierdź hasło musi zawierać od 4 do 30 znaków")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "hasla nie są jednakowe")]
        [Display(Name = "Potwierdz hasło: ")]
        public string passwordConfirmation { set; get; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Hasło jest wymagane")]
        [Display(Name = "Hasło: ")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "pole - hasło musi zawierać od 4 do 30 znaków")]
        public string Password { get { return user.Password; } set { user.Password = value; } }
        public string CompanyName { set; get; }
        [Display(Name = "Rola użytkownika: ")]
        public string userRoleName { set; get; }
        public SelectList RoleSelectList { set; get; }

    }
}