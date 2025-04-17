using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Commands
{
    public class RegisterProjectCommand
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
       


        public required string CreatedByUserMail{ get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            { throw new ValidationException(" UserName must not be empty "); }
            if (string.IsNullOrWhiteSpace(Description))
            { throw new ValidationException(" Description must not be empty "); }
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailPattern);
            if ((!regex.IsMatch(CreatedByUserMail)))
            { throw new ValidationException("Enter correct Email format"); }
            

        }
    }
}
