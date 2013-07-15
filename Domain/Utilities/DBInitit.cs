using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Domain.Concrete;
using Domain.Entities;
namespace Domain.Utilities
{
    public class DBInitit : DropCreateDatabaseIfModelChanges<EFDbContext>
    {
        protected override void Seed(EFDbContext context)
        {
            var userRoles = new List<UserRole>
            {
                new UserRole { RoleName = "Admin" },
                new UserRole { RoleName = "Customer"}
            };
            userRoles.ForEach(us => context.UserRoles.Add(us));
            context.SaveChanges();
        }
    
    }
}
