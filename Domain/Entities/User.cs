using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public virtual UserRole UserRole { set; get; }
        public virtual Company Company { set; get; } 
        [Required]
        [StringLength(30,MinimumLength=3, ErrorMessage = "pole - nazwa użytkownika musi zawierać od 3 do 30 znaków")]
        public string Login { set; get; }
        [Required]
        [MinLength(6,ErrorMessage="Hasło musi posiadać co najmniej 6 znaków")]
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
