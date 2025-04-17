using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Commands
{
    public class OpenProjectCommand
    {
        public required int Id { get; set; }
        
        public required DateOnly EndDate { get; set; }

        public void Validate()
        {
            if(Id <= 0)
            { throw new ValidationException(" Id must be more than zero "); }
            
            if(EndDate <= DateOnly.FromDateTime(DateTime.UtcNow))
            { throw new ValidationException(" End date must not be equal or less than current date time "); }

        }

    }
}
