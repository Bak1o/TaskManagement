using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Commands
{
    public class CloseProjectCommand
    {
        public required int Id { get; set; }
        public void Validate()
        {
            if (Id <= 0)
            { 
                throw new ValidationException("Id must not be less than or equal to zero");
            }
        }
    }
}
