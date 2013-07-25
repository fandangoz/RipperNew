using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utilities
{
    public class _ItemNotExistInDatabaseException :Exception
    {
        public _ItemNotExistInDatabaseException() {  }
        public _ItemNotExistInDatabaseException(string message) : base(message) { }
    }
}
