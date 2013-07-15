using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFOrderStatusesRepository : IOrderStatusesRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<OrderStatus> OrderStatuses
        {
            get { return context.OrderStatuses; }
        }
    }
}
