using Domain.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace Domain.Concrete
{
    public class EFDbContext : DbContext
    {        
        public DbSet<Company> Companies { set; get; }
        public DbSet<EquipmentType> EquipmentTypes { set; get; }
        public DbSet<Order> Orders { set; get; }
        public DbSet<OrderStatus> OrderStatuses { set; get; }
        public DbSet<User> Users { set; get; }
        public DbSet<UserRole> UserRoles { set; get; }


    }
}
