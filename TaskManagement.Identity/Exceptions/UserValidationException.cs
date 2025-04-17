using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Identity.Exceptions
{
    public class UserValidationException : Exception
    {
        public UserValidationException() : base()
        {

        }
        public UserValidationException(string message) : base(message)
        {
        }

    }
}
