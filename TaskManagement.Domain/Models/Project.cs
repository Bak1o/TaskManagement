using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models.Abstraction;
using TaskManagement.Domain.Models.Enums;

namespace TaskManagement.Domain.Models
{
    public class Project : DomainEntity<int>
    {

        public string Name { get;private set; }
        public string Description { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public Status Status { get; private set; }
        public int CreatedByCustomerId { get; private set; }
        public Customer CreatedByCustomer { get;  set; } = null!;
       
        // One Project has many Tasks
        public List<DomainTask> Tasks { get; set; } = new(); 

        public Project(string name, string description,  int createdByCustomerId)
        {
            Name = name;
            Description = description;
            
            Status = Status.ToDo;
            CreatedByCustomerId = createdByCustomerId;

            
        }

        public void Open(DateTime endDate)
        {
            Status = Status.InProgress;
            StartDate = DateTime.UtcNow;
            EndDate = endDate;
            
        }

        public void Close()
        {
            Status = Status.Done;
        }

        public void Suspend()
        {
            Status = Status.Suspended;
        }



    }
}
