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
        IQueryable<EquipmentType> EquipmentTypes { get; }
        void saveEquipment(EquipmentType eqType);
        void editEQType(EquipmentType eqType);
    }
}
