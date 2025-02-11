using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models.Abstraction;
using TaskManagement.Domain.Models.Enums;

namespace TaskManagement.Domain.Models
{
    public class UpdateProject : DomainEntity<int>
    {

        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime EndDate { get; set; }
        public required Status Status { get; set; }

        public void Validate()
        {
            if (Name!.Length is < 1 or > 100)
                throw new ValidationException(" project name must contain minimum 1 symbol and maximum 100 symbol ");

            if (Description!.Length is < 1 or > 4000)
                throw new ValidationException(" project description must contain minimum 1 symbol and maximum 4000 symbol ");

        }
    }
}
