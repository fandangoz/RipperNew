using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;
using Domain.Utilities;
namespace Domain.Concrete
{
    public class EFEquipmentRepository :IEquipmentTypesRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<EquipmentType> EquipmentTypes
        {
            get { return context.EquipmentTypes; }

        }
        public void saveEquipment(EquipmentType eqType)
        {
            if(context.EquipmentTypes.FirstOrDefault(eq => eq.EquipmentTypeName == eqType.EquipmentTypeName) != null)
            {
                throw new UserExistInDatabaseException("dany typ sprzętu znajduje się już w bazie danych");
            }
            context.EquipmentTypes.Add(eqType);
            context.SaveChanges();
        }
        public void editEQType(EquipmentType eqType)
        {
            EquipmentType dbEqType = context.EquipmentTypes.FirstOrDefault(eq => eq.EquipmentTypeID == eqType.EquipmentTypeID);
            {
                dbEqType.EquipmentTypeName = eqType.EquipmentTypeName;
                context.Entry(dbEqType).State = System.Data.EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
