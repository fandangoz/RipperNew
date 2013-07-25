using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class ReperairTypePrice
    {
        [Key]
        public int ReperairTypePriceID { set; get; }
        [Required]
        public virtual ReperairType ReperairType { set; get; }
        [Required]
        [DataType(DataType.Currency)]
        public double Price { set; get; }
    }
}
