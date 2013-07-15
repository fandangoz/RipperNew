using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class EquipmentType
    {
        [Key]
        public int EquipmentTypeID { set; get; }
        [Required(ErrorMessage = "pole - nazwa typ sprzętu nie może być puste")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "pole - typ sprzętu musi zawierać od 3 do 30 znaków")]
        [Display(Name = "Nazwa kategorii: ")]
        public string EquipmentTypeName { set; get; }
    }
}
