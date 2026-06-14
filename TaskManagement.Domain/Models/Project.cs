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
        public DateOnly? StartDate { get; private set; }
        public DateOnly? EndDate { get; private set; }
        public Status Status { get; private set; }
        public string CreatedByUserId { get; private set; }
        //public ApplicationUser CreatedByUser { get;  set; } = null!;
       
        // One Project has many Tasks
        //public List<DomainTask> Tasks { get; set; } = new(); 

        public Project(string name, string description,  string createdByUserId)
        {
            Name = name;
            Description = description;
            
            Status = Status.ToDo;
            CreatedByUserId = createdByUserId;

            
        }

        public void Open(DateOnly endDate)
        {
            Status = Status.InProgress;
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow);
            EndDate = endDate;
            
        }

        public void Close()
        {
            Status = Status.Done;
            EndDate =DateOnly.FromDateTime(DateTime.UtcNow);
        }

        public void Suspend()
        {
            Status = Status.Suspended;
        }



    }
}
