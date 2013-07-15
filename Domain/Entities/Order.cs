using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        [Required]
        [Display(Name = "Nazwa klienta: ")]
        public virtual User Customer { set; get; }
        [Required]
        [Display(Name = "Kategoria naprawianego sprzętu: ")]
        public virtual EquipmentType EquipmentType { get; set; }
        [Required]
        [Display(Name = "Status zamówienia: ")]
        public virtual OrderStatus OrderStatus { get; set; }
        [Range(0.0, double.MaxValue)]
        [Display(Name = "Cena: ")]
        public double cost { set; get; }
        [DataType(DataType.DateTime)]
        [Required]
        [Display(Name = "Data przyjęcia: ")]
        public DateTime receivingDate { set; get; }
        [Display(Name = "Data zakończenia: ")]
        public DateTime endDate { set; get; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Dodatkowe informacje: ")]
        public string additionalInformation { set; get; }
    }
}
