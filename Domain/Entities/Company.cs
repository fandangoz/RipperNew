using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    class Company
    {
        //TODO company and user 1 account or more?
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

    }
}
