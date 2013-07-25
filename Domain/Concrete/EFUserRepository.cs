using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Abstract;
using SimpleCrypto;
using Domain.Utilities;
namespace Domain.Concrete
{
    public class EFUserRepository :IUserRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<User> Users
        {
            get { return context.Users; }
        }
        public void SaveUser(User user)
        {
            User dbUser = context.Users.FirstOrDefault(u => u.UserID == user.UserID);
            if (context.Users.FirstOrDefault(u => u.Login == user.Login) != null)
            {
                throw (new UserExistInDatabaseException("Podany login jest zajety"));
            }
            user.UserRole = context.UserRoles.FirstOrDefault(uR => uR.UserRoleID == user.UserRole.UserRoleID);
            if (user.UserRole == null)
            {
                throw (new UserExistInDatabaseException("Wybrana rola nie istnieje"));
            }
            if (user.Company != null)
            {
                user.Company = context.Companies.FirstOrDefault(c => c.CompanyID == user.Company.CompanyID);
            }

            if (user.UserID == 0)
            {
                var crypto = new SimpleCrypto.PBKDF2();
                var encryptPass = crypto.Compute(user.Password);
                user.Password = encryptPass;
                user.PasswordSalt = crypto.Salt;
                context.Users.Add(user);
            }
            context.SaveChanges();
        }

        public void EditUser(User u)
        {
            User user = context.Users.FirstOrDefault(us => us.UserID == u.UserID);
            user.Name = u.Name;
            user.Login = u.Login;
            user.Surname = u.Surname;
            user.Address = u.Address;
            user.additionalInformation = u.additionalInformation;
            user.Phone = u.Phone;
            user.isActive = u.isActive;
            user.UserRole = context.UserRoles.FirstOrDefault(uR => uR.UserRoleID == u.UserRole.UserRoleID);
            if (user.UserRole == null)
            {
                throw (new UserExistInDatabaseException("Wybrana rola nie istnieje"));
            }
            if (u.Company != null)
            {
                user.Company = context.Companies.FirstOrDefault(c => c.CompanyID == u.Company.CompanyID);
                context.Entry(user.Company).State = System.Data.EntityState.Modified;
            }
            else
                if (user.Company != null)
                {
                    user.Company = null;

                }
            context.Entry(user).State = System.Data.EntityState.Modified;
            
            //context.Entry(user.UserRole).State = System.Data.EntityState.Modified;
            context.SaveChanges();
        }

        public User ChangeUserPassword(string userLogin, string oldPassword, string newPassword)
        {
            User user = IsValid(userLogin, oldPassword);
            if (user != null)
            {
                var crypto = new SimpleCrypto.PBKDF2();
                var encryptPass = crypto.Compute(newPassword);
                user.Password = encryptPass;
                user.PasswordSalt = crypto.Salt;
                user.UserRole = context.UserRoles.FirstOrDefault(uR => uR.UserRoleID == user.UserRole.UserRoleID);
                if (user.Company != null)
                {
                    user.Company = context.Companies.FirstOrDefault(c => c.CompanyID == user.Company.CompanyID);
                }
                context.SaveChanges();
            }
            return user;
        }
        public void ChangeUserRole(User u, string newRoleName)
        {
            User user = context.Users.FirstOrDefault(us => us.UserID == u.UserID);
            user.UserRole = context.UserRoles.FirstOrDefault(uR => uR.RoleName == newRoleName);
            if (user.UserRole == null)
            {
                throw (new UserExistInDatabaseException("Wybrana rola nie istnieje"));
            }
            context.Entry(user).State = System.Data.EntityState.Modified;
            context.Entry(user.UserRole).State = System.Data.EntityState.Modified;
            context.SaveChanges();
        }

        public User IsValid(string login, string password)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            User user = context.Users.FirstOrDefault(u => u.Login == login);
            if (user != null)
            {
                if (user.Password == crypto.Compute(password, user.PasswordSalt))
                {
                    return user;
                }
            }
            return null;
        }

    }
}
