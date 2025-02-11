using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Commands
{
    public class RegisterProjectCommand
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required DateTime StartDate { get; set; }


        public required int CreatedByUserId { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            { throw new ValidationException(" UserName must not be empty "); }
            if (string.IsNullOrWhiteSpace(Description))
            { throw new ValidationException(" Description must not be empty "); }
            if (CreatedByUserId <= 0)
            { throw new ValidationException(" Created By User Id must not be zero or less than zero "); }
            

        }
    }
}
