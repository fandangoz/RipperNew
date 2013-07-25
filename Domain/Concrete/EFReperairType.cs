using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;
namespace Domain.Concrete
{
    public class EFReperairType : IReperairType
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<ReperairType> ReperairTypes
        {
            get
            {
                return context.ReperairTypes;
            }
        }

        public void SaveReperairType(ReperairType reperairType)
        {
            context.ReperairTypes.Add(reperairType);
            context.SaveChanges();
        }
    }
}
