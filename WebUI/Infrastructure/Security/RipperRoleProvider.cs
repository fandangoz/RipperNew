using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
namespace WebUI.Infrastructure.Security
{
    public class RipperRoleProvider : RoleProvider
    {
        private IUserRepository repo;
        public RipperRoleProvider()
        {
            repo = new EFUserRepository();
        }
        public override string[] GetRolesForUser(string username)
        {

                var objUser = repo.Users.FirstOrDefault(x => x.Login == username);
                if (objUser == null)
                {
                    return null;
                }
                else
                {
                    string[] ret = new string[1];
                    ret[0] = objUser.UserRole.RoleName;
                    return ret;
                }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {

            User user = repo.Users.FirstOrDefault(u => u.Login.Equals(username, StringComparison.CurrentCultureIgnoreCase));
            var roles = user.UserRole.RoleName;
                if (user != null)
                    return roles.Equals(roleName);
                else
                    return false;

        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }


    }
}