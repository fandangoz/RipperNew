using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;
namespace Domain.Concrete
{
    public class EFUserRoleRepository : IUserRolesRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<UserRole> UsersRoles  
        {
            get { return context.UserRoles; }
        }
        public void Add(UserRole userRole)
        {
            context.UserRoles.Add(userRole);
            context.SaveChanges();
        }


    }
}
