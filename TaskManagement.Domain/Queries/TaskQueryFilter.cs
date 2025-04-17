using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models.Enums;

namespace TaskManagement.Domain.Queries
{
    public class TaskQueryFilter
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AssignedUserMail { get; set; }
        public Status? Status { get; set; }
        public DateOnly? Deadline { get; set; }
        public DateOnly? DoneDate { get; set; }
        public string? SortBy { get; set; } 

    }
}
