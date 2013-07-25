using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        void SaveUser(User user);
        User ChangeUserPassword(string userLogin, string oldPassword, string newPassword);
        User IsValid(string login, string password);
        void EditUser(User user);
        void ChangeUserRole(User user, string newRoleName);
    }
}
