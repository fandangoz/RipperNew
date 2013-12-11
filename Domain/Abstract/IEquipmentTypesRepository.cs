using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
namespace Domain.Abstract
{
    public interface IEquipmentTypesRepository
    {
        void saveEquipment(EquipmentType eqType);
        void editEQType(EquipmentType eqType);
        IQueryable<EquipmentType> EquipmentTypes { get;}
    }
}
