using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Domain.Entities
{

    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public virtual UserRole UserRole { set; get; }
        public virtual Company Company { set; get; } 
        [Required(ErrorMessage="pole - nazwa użytkownika jest wymagane")]
        [StringLength(30,MinimumLength=4, ErrorMessage = "pole - nazwa użytkownika musi zawierać od 4 do 30 znaków")]
        public string Login { set; get; }
        [Required(ErrorMessage="Pole hasło jest wymagane")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło: ")]
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        [Display(Name = "Imie: ")]
        public string Name { set; get; }
        [Display(Name = "Nazwisko: ")]
        public string Surname { set; get; }
        [Display(Name = "Adres: ")]
        public string Address { set; get; }
        [Display(Name = "Numer telefonu: ")]
        public string Phone { set; get; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Dodatkowe informacje: ")]
        public string additionalInformation { set; get; }

    }
}
