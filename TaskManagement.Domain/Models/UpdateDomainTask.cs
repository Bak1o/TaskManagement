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
    public class UpdateDomainTask : DomainEntity<int>
    {

        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<Customer>? AssignedUsers { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime DeadLine { get; set; }

        public void Validate()
        {
            if (Title!.Length is < 1 or > 100)
                throw new ValidationException(" project name must contain minimum 1 symbol and maximum 100 symbol ");

            if (Description!.Length is < 1 or > 4000)
                throw new ValidationException(" project description must contain minimum 1 symbol and maximum 4000 symbol ");

            if (Status == Status.Done)
            {
                DeadLine = DateTime.Today;
            }


        }

    }
}
