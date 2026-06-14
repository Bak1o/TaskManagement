using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Service.DataTransferObjects
{
    public sealed class TaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly Deadline { get; set; }

        public string CreatedByUserEmail { get; set; } = string.Empty;
    }
}
