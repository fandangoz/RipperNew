using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;
using Domain.Utilities;
namespace Domain.Concrete
{
    public class EFCompanyRepository : ICompaniesRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Company> Companies
        {
            get { return context.Companies; }
        }
        public void SaveCompany(Company company)
        {
            if (company.CompanyID == 0)
            {
                if(context.Companies.FirstOrDefault( c => c.CompanyName.ToLower() == company.CompanyName.ToLower())!= null)
                {
                    throw (new UserExistInDatabaseException("Podana nazwa firmy jest zajeta"));
                }
                context.Companies.Add(company);
            }
            else
            {
                if (context.Companies.FirstOrDefault(c => (c.CompanyID != company.CompanyID) && c.CompanyName.ToLower() == company.CompanyName.ToLower()) != null)
                {
                    throw (new UserExistInDatabaseException("Podany nazwa firmy jest zajeta"));
                }
                Company dbCompany = context.Companies.FirstOrDefault(c => c.CompanyID == company.CompanyID);
                dbCompany.CompanyName = company.CompanyName;
                dbCompany.CompanyRegon = company.CompanyRegon;
                dbCompany.CompanyAddress = company.CompanyAddress;
            }
            context.SaveChanges();
        }
    }
}
