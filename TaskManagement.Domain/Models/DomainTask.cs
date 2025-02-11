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
    public class DomainTask : DomainEntity<int>
    {

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        public List<Customer> AssignedCustomers { get; set; } = new();
        public Status Status { get; set; }
        public required Priority Priority { get; set; }
        public DateTime StartDate { get; private set; }
        public required DateTime DeadLine { get; set; }
        public required int CreatedByCustomerId { get; set; }
        public Customer CreatedByCustomer { get; set; } = null!;
        

        public void Validate()
        {
            if (Title.Length is < 1 or > 100)
                throw new ValidationException(" project name must contain minimum 1 symbol and maximum 100 symbol ");

            if (Description.Length is < 1 or > 4000)
                throw new ValidationException(" project description must contain minimum 1 symbol and maximum 4000 symbol ");

            if (DeadLine <= DateTime.Now)
                throw new ValidationException(" Deadline must be in future time ");
        }

        public void Open()
        {
            StartDate = DateTime.Now;
            Status = Status.ToDo;
            

        }

        public void close()
        {
            DeadLine = DateTime.Now;
            Status = Status.Done;
        }










    }
}
