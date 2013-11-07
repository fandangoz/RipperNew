using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }
        [Required]
        [Display(Name = "Nazwa firmy: ")]
        public string CompanyName { get; set; }
        [Display(Name = "Adres firmy: ")]
        public string CompanyAddress { get; set; }
        [Display(Name = "Regon: ")]
        public string CompanyRegon { get; set; }
        [Display(Name = "Dodatkowe informacje: ")]
        public string AdditionalData { get; set; }
        [Required]
        public bool isActive { set; get; }
    }
}
