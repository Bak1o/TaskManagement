using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Service.DataTransferObjects
{
    public sealed class TaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; } // Convert to string
        public string Priority { get; set; } // Convert to string
        public DateOnly StartDate { get; set; }
        public DateOnly Deadline { get; set; }

        public string CreatedByUserEmail { get; set; }
    }
}
