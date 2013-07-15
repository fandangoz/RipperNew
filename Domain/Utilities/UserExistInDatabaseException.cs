using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Utilities
{
    public class UserExistInDatabaseException :Exception
    {
        public UserExistInDatabaseException() {  }
        public UserExistInDatabaseException(string message) : base(message) { }

    }
}
