using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Abstract;
namespace Domain.Concrete
{
    class EFUserRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<User> User
        {
            get { return context.Users; }
        }
    }
}
