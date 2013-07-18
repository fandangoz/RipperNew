using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace WebUI.Models
{
    public class PasswordChangeViewModel
    {
        [Required(ErrorMessage = "pole stare hasło jest wymagane")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "hasło musi zawierać przynajmniej 6 znaków")]
        [Display(Name = "Stare hasło")]
        [DataType(DataType.Password)]
        public string OldPassword { set; get; }
        [Required(ErrorMessage = "pole nowe stare hasło jest wymagane")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "hasło musi zawierać przynajmniej 6 znaków")]
        [Display(Name = "Nowe hasło")]
        [DataType(DataType.Password)]
        public string NewPassword { set; get; }
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "hasla nie są jednakowe")]
        [Display(Name = "Potwierdz hasło: ")]
        public string NewPasswordConfirmation { set; get; }
    }
}