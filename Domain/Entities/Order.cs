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
        public Order()
        {
            ReperairCostList = new List<ReperairTypePrice>();
        }
        [Key]
        public int OrderID { get; set; }
        [Display(Name = "Nazwa klienta: ")]
        public virtual User Customer { set; get; }
        public virtual Company Company { set; get; }
        [Required(ErrorMessage="Pole typ sprzetu jest wymagane")]
        [Display(Name = "Kategoria naprawianego sprzętu: ")]
        public virtual EquipmentType EquipmentType { get; set; }
        [Required]
        [Display(Name = "Status zamówienia: ")]
        public virtual OrderStatus OrderStatus { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data przyjęcia: ")]
        public DateTime? receivingDate { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data zakończenia: ")]
        public DateTime? endDate { set; get; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Opis zlecenia: ")]
        public string Description { set; get; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Uwagi: ")]
        public string additionalInformation { set; get; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Nota po zakończeniu: ")]
        public string endNote { set; get; }
        public virtual ICollection<ReperairTypePrice> ReperairCostList { set; get; }
        [Required]
        public bool isCompany { set; get; }

    }
}
