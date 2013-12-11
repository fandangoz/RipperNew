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
        EFUserRepository userRepo = new EFUserRepository();
        EFCompanyRepository companyRepo = new EFCompanyRepository();
        EFOrderRepository orderrepo = new EFOrderRepository();
        protected override void Seed(EFDbContext context)
        {
            var userRoles = new List<UserRole>
            {
                new UserRole { RoleName = "Admin" },
                new UserRole { RoleName = "Klient"},
                new UserRole { RoleName = "Biuro" }
            };
           

            userRoles.ForEach(us => context.UserRoles.Add(us));
            context.SaveChanges();
             OrderStatus orderStatus = new OrderStatus { OrderStatusName = "Przyjęte" };
            var orderStatuses = new List<OrderStatus> {
                orderStatus,
                  new OrderStatus { OrderStatusName = "Oczekuje na decyzje" },
                  new OrderStatus { OrderStatusName = "Gotowe do odbioru" },
                  new OrderStatus { OrderStatusName = "Anulowane" },
                  new OrderStatus { OrderStatusName = "Zakończone" },
            };
            orderStatuses.ForEach(os => context.OrderStatuses.Add(os));
            context.SaveChanges();
            EquipmentType typSprzetu = new EquipmentType { EquipmentTypeName = "Piła"};

            context.EquipmentTypes.Add(typSprzetu);
            context.SaveChanges();
            userRepo.SaveUser(new User { isActive = true, Login = "Admin", Password = "Admin1", UserRole = context.UserRoles.FirstOrDefault(ur => ur.RoleName == "Admin"), Surname = "Admin", Name = "Admin" });
            userRepo.SaveUser(new User { isActive = true, Login = "Biuro", Password = "Biuro1", UserRole = context.UserRoles.FirstOrDefault(ur => ur.RoleName == "Biuro"), Surname = "BiuroTest", Name = "BiuroTest" });
            User klient = new User { isActive = true, Login = "Klient1", Password = "Klient1", UserRole = context.UserRoles.FirstOrDefault(ur => ur.RoleName == "Klient"), Surname = "KlientTest", Name = "KlientTest" };
            User klient1 = new User { isActive = true, Login = "Klient2", Password = "Klient2", UserRole = context.UserRoles.FirstOrDefault(ur => ur.RoleName == "Klient"), Surname = "KlientTest", Name = "KlientTest" };
           
            userRepo.SaveUser(klient);
            userRepo.SaveUser(klient1);
            companyRepo.SaveCompany(new Company { isActive = true, CompanyName = "Firma1", CompanyRegon = "1123222" });

            var reperairTypes = new List<ReperairType> {
            new ReperairType { Description = "przegląd" },
            new ReperairType { Description = "wymiana świecy" }
            };
            reperairTypes.ForEach(rt => context.ReperairTypes.Add(rt));
            context.SaveChanges();
            var reperairTypePrices = new List<ReperairTypePrice> {
                 new ReperairTypePrice { ReperairType = reperairTypes[0], Price = 30 }, 
                 new ReperairTypePrice { ReperairType = reperairTypes[1], Price = 30 }
            };

            orderrepo.SaveOrder(new Order { isCompany = false, Customer = klient, OrderStatus = orderStatus, EquipmentType = typSprzetu, ReperairCostList = reperairTypePrices, Description = "opis zlecenia" });
            orderrepo.SaveOrder(new Order { isCompany = false, Customer = klient1, OrderStatus = orderStatus, EquipmentType = typSprzetu, ReperairCostList = reperairTypePrices, Description = "opis zlecenia" });

            Company company = new Company { isActive = true, CompanyName = "Firma testowa" };
            companyRepo.SaveCompany(company);

            User klientFirma = new User { isActive = true, Login = "Klient3", Password = "Klient3", UserRole = context.UserRoles.FirstOrDefault(ur => ur.RoleName == "Klient"), Surname = "KlientTest", Name = "KlientTest", Company = company };

            orderrepo.SaveOrder(new Order { isCompany = true, Company = company, OrderStatus = orderStatus, EquipmentType = typSprzetu, ReperairCostList = reperairTypePrices, Description = "opis zlecenia" });

        }

    
    }
}
