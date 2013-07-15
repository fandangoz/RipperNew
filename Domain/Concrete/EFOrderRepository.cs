using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFOrderRepository : IOrdersRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Order> Orders 
        {
            get { return context.Orders; }
        }
    }
}
