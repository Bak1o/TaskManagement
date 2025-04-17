using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models.Enums;
using TaskManagement.Domain.Models;
using TaskManagement.Identity.Models;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Commands
{
    public class OpenTaskCommand
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int ProjectId { get; set; }
        
        public required List<string> AssignedUsersEmails { get; set; } 
        
        public required Priority Priority { get; set; }
        
        public required DateOnly DeadLine { get; set; }
        public required string CreatedByUserMail { get; set; }

        public void Validate()
        {
            if (Title.Length is < 1 or > 100)
                throw new ValidationException(" project name must contain minimum 1 symbol and maximum 100 symbol ");

            if (Description.Length is < 1 or > 4000)
                throw new ValidationException(" project description must contain minimum 1 symbol and maximum 4000 symbol ");

            if (DeadLine <= DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ValidationException(" Deadline must be in future time ");
        }
    }
}
