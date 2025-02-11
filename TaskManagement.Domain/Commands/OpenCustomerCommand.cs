using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models.Enums;

namespace TaskManagement.Domain.Commands
{
    public class OpenCustomerCommand
    {
        public required int Id { get; set; }
        public required Role Role { get; set; }

        public void Validate()
        {
            if (Id <= 0)
            {
                throw new ValidationException("Id must not be less than or equal to zero");
            }

            if (!Enum.IsDefined(typeof(Role), Role))
            {
                throw new ValidationException("Role is not valid");
            }
        }
    }
}
