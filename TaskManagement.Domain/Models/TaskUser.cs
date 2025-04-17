using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Identity.Models;

namespace TaskManagement.Domain.Models
{
    public class TaskUser
    {
        public int DomainTaskId { get; set; }
        public DomainTask DomainTask { get; set; } = null!;

        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
