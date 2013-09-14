using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BootstrapMvcSample.Controllers;
using NavigationRoutes;
using WebUI.Controllers;
namespace BootstrapMvcSample
{
    public class ExampleLayoutsRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapNavigationRoute<UsersController>("Menu zarządu", c => c.Index())
                  .AddChildRoute<UsersController>("Dodaj użytkownika", c => c.Create())
                ;
        }
    }
}
