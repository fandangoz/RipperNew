using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BootstrapMvcSample.Controllers;
using NavigationRoutes;
using WebUI.Controllers;
using System.Web.Security;

namespace BootstrapMvcSample
{
    public class ExampleLayoutsRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.MapNavigationRoute<UsersController>("Użytkownicy", c => c.BootIndex(),"Admin,Biuro").FilterRoute("Admin")
                  .AddChildRoute<UsersController>("Dodaj użytkownika", c => c.Create())
                  .AddChildRoute<UsersController>("Wyświetl użytkowników", c => c.Index());
 
                routes.MapNavigationRoute<CompanyController>("Firmy", c => c.BootIndex(),"Admin,Biuro").FilterRoute("Biuro")
                  .AddChildRoute<CompanyController>("Dodaj firmę", c => c.Create())
                  .AddChildRoute<CompanyController>("Wyświetl firmy", c => c.Index());

                routes.MapNavigationRoute<OrdersController>("Zlecenia", c => c.BootIndex(), "Admin,Biuro")
                    .AddChildRoute<OrdersController>("Dodaj zlecenie", c => c.CreateBoot())
                    .AddChildRoute<OrdersController>("Wyświetl zlecenia", c => c.Index());

                routes.MapNavigationRoute<EquipmentTypeController>("Typy sprzętu", c => c.BootIndex(), "Admin,Biuro")
                    .AddChildRoute<EquipmentTypeController>("Dodaj typ sprzętu", c=> c.Create())
                    .AddChildRoute<EquipmentTypeController>("Wyświetl typy sprzętu", c=> c.Index());



                routes.MapNavigationRoute<AccountController>("Panel użytkownika", c => c.BootIndex(), "Admin,Biuro,Klient").FilterRoute("Klient")
                    .AddChildRoute<AccountController>("zmień hasło", c => c.ChangePassword())
                       .AddChildRoute<AccountController>("wyloguj", c => c.LogOut());
              
        }
    }
}
