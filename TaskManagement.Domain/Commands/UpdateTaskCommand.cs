using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models.Enums;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Commands
{
    public class UpdateTaskCommand
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public List<Customer>? AssignedCustomers { get; set; }
        public Status? Status { get; set; }
        public Priority? Priority { get; set; }
        public DateTime? DeadLine { get; set; }

        public void Validate()
        {
            if (Title!.Length is < 1 or > 100)
                throw new ValidationException(" project name must contain minimum 1 symbol and maximum 100 symbol ");

            if (Description!.Length is < 1 or > 4000)
                throw new ValidationException(" project description must contain minimum 1 symbol and maximum 4000 symbol ");




        }

        /// <summary>
        /// validate Status with pre update model
        /// </summary>
        /// <param name="task"></param>
        /// <exception cref="ValidationException"></exception>
        public void UpdateMatchingCheck(DomainTask task)
        {
            if (Status is not null)
            {
                if (task.Status == Models.Enums.Status.InProgress && Status == Models.Enums.Status.ToDo)
                { throw new ValidationException(" if task status is in progress you can't change into to do"); }

                if (task.Status == Models.Enums.Status.Done && (Status == Models.Enums.Status.ToDo || Status == Models.Enums.Status.InProgress))
                    { throw new ValidationException(" if task status is done you can't change status "); }

                task.Status = (Status)Status;

            }

            if (Priority is not null)
            {
                task.Priority = (Priority)Priority;
            }

            if (DeadLine is not null)
            {
                task.DeadLine = (DateTime)DeadLine;
            }
            
            
            if (AssignedCustomers is not null)
            {
                task.AssignedCustomers = AssignedCustomers;
            }

            task.Title = Title;
            task.Description = Description;

            task.Id = Id;

        }
    }
}
