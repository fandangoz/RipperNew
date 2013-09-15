using System;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using NavigationRoutes;
using Microsoft.Web.Mvc;
namespace WebUI.Infrastructure.Concrete
{
    public class NaviFilter : INavigationRouteFilter
    {
        private NavigationRouteBuilder builder;
        private string role;

        public NaviFilter(NavigationRouteBuilder builder, string role)
        {
            this.builder = builder;
            this.role = role;
        }

        public bool ShouldRemove(Route navigationRoutes)
        {
            if (navigationRoutes is NamedRoute)
            {
                string area = AreaHelpers.GetAreaName(navigationRoutes);

                if (area == null)
                {
                    return false;
                }


                string userRoles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name).FirstOrDefault();
                if (area.ToLower().Contains(this.role.ToLower()) && userRoles != null && userRoles.ToLower().Equals(role.ToLower()) )
                {
                    return false;
                }
            }

            return true;
        }
    }
}