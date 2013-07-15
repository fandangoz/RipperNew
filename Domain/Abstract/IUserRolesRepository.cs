using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
namespace Domain.Abstract
{
    interface IUserRolesRepository
    {
        IQueryable<UserRole> UsersRoles { get; }
    }
}
