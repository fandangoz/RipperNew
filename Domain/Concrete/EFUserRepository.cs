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
            else
            {

                dbUser.Name = user.Name;
                dbUser.Phone = user.Phone;
                dbUser.Surname = user.Surname;
                dbUser.Address = user.Address;
                dbUser.additionalInformation = user.additionalInformation;
                dbUser.Company = user.Company;
            }
            context.SaveChanges();
        }

        public void ChangeUserPassword(User user, string newPassword)
        {
                var crypto = new SimpleCrypto.PBKDF2();
                var encryptPass = crypto.Compute(newPassword);
                user.Password = encryptPass;
                user.PasswordSalt = crypto.Salt;
                user.UserRole = user.UserRole;
                context.SaveChanges();

        }

    }
}
