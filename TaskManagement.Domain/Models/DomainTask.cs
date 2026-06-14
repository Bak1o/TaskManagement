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
        public List<TaskUser> AssignedUsers { get; set; } = new();
        public Status Status { get; set; }
        public required Priority Priority { get; set; }
        public DateOnly StartDate { get; private set; }
        public required DateOnly DeadLine { get; set; }
        public required string CreatedByUserId { get; set; }
        //public string CreatedByUser { get; set; } = null!;

        public DomainTask()
        {
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow);
            Status = Status.ToDo;


        }
        public void Validate()
        {
            if (Title.Length is < 1 or > 100)
                throw new ValidationException(" project name must contain minimum 1 symbol and maximum 100 symbol ");

            if (Description.Length is < 1 or > 4000)
                throw new ValidationException(" project description must contain minimum 1 symbol and maximum 4000 symbol ");

            if (DeadLine <= DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ValidationException(" Deadline must be in future time ");
        }

        //public void Open()
        //{
        //    StartDate = DateTime.Now;
        //    Status = Status.ToDo;
            

        //}

        public void close()
        {
            DeadLine = DateOnly.FromDateTime(DateTime.UtcNow);
            Status = Status.Done;
        }










    }
}
