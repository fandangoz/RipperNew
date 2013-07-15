using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserRole
    {
        [Key]
        public int UserRoleID { set; get; }
        [Required(ErrorMessage = "pole - nazwa roli nie może być puste")]
        [StringLength(30,MinimumLength=3,ErrorMessage="nazwa roli musi zawierać od 3 do 30 znaków")]
        [Display(Name = "Nazwa roli: ")]
        public string RoleName { set; get; }
    }
}
