using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;
namespace WebUI.Models
{
    public class CompanyUsersViewModel
    {
        public User[] Users { set; get; }
        public Company Company { set; get; }
    }
}