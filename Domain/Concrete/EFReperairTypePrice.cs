using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;
namespace Domain.Concrete
{
    public class EFReperairTypePrice : IReperairTypePrice, Domain.Abstract.IReperairType
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<ReperairTypePrice> ReperairTypePrice
        {
            get { return context.ReperairTypePrice; }
        }

        public IQueryable<ReperairType> ReperairTypes
        {
            get { throw new NotImplementedException(); }
        }

        public void SaveReperairType(ReperairType reperairType)
        {
            throw new NotImplementedException();
        }
    }
}
