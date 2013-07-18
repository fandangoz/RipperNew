using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage="proszę podać login")]
        [StringLength(30,MinimumLength=4,ErrorMessage ="Login powinien zawierać conajmniej 4 znaki")]
        [Display(Name="Login:")] 
        public string Login { set; get; }
        [Required(ErrorMessage="pole hasło jest wymagane")]
        [StringLength(30,MinimumLength=6, ErrorMessage="hasło musi zawierać przynajmniej 6 znaków")]
        [Display(Name="Hasło:")]
        [DataType(DataType.Password)]
        public string Password { set; get; }

    }
}