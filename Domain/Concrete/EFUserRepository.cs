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
            //TO DO reference to company
            if (user.UserID == 0)
            {
                if(context.Users.FirstOrDefault(u => u.Login == user.Login) != null)
                {
                    throw ( new UserExistInDatabaseException("Podany login jest zajety"));
                }
                var crypto = new SimpleCrypto.PBKDF2();
                var encryptPass = crypto.Compute(user.Password);
                user.Password = encryptPass;
                user.PasswordSalt = crypto.Salt;
                context.Users.Add(user);
            }
            else
            {
                User dbUser = context.Users.FirstOrDefault(u => u.UserID == user.UserID);
                dbUser.Name = user.Name;
                dbUser.Phone = user.Phone;
                dbUser.Surname = user.Surname;
                dbUser.Address = user.Address;
                dbUser.additionalInformation = user.additionalInformation;
                dbUser.Company = user.Company;
            }
            context.SaveChanges();
        }

        public void ChangeUserPassword(string userLogin, string newPassword, string oldPassword)
        {

        }

    }
}
