using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFEquipmentRepository :IEquipmentTypesRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<EquipmentType> EquipmentTypes
        {
            get { return context.EquipmentTypes; }
        }
    }
}
