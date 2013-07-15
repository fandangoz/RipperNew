using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class OrderStatus
    {
        [Key]
        public int OrderStatusID { get; set; }
        [Required(ErrorMessage = "pole - opis statusu zamówienia nie może być puste")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "nazwa statusu zamówienia musi zawierać od 3 do 30 znaków")]
        [Display(Name = "Nazwa statusu zamówienia: ")]
        public string OrderStatusName { set; get; }
    }
}
