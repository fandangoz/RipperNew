using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
namespace Domain.Abstract
{
    public interface ICompaniesRepository
    {
        IQueryable<Company> Companies { get; }
        void SaveCompany(Company company);
    }

}
