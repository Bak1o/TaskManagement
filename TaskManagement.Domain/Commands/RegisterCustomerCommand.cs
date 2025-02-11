using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Commands
{
    public sealed class RegisterCustomerCommand
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            { throw new ValidationException(" UserName must not be empty "); }
            if (string.IsNullOrWhiteSpace(Email))
            { throw new ValidationException(" Email must not be empty "); }
            if (!string.IsNullOrWhiteSpace(Password))
            { throw new ValidationException(" Password must not be empty "); }
            if (!string.IsNullOrWhiteSpace(FirstName))
            { throw new ValidationException(" First Name must not be empty "); }
            if (!string.IsNullOrWhiteSpace(LastName))
            { throw new ValidationException(" Last Name must not be empty "); }


        }


    }
}
