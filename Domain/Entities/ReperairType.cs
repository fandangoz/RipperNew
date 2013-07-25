using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class ReperairType
    {
        [Key]
        public int ReperairTypeID { set; get; }
        [Required]
        public string Description { set; get; }
    }
}
