using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Commands
{
    public sealed class AssignUserCommands
    {
        public required int TaskId { get; set; }
        public required string ApplicationUserEmail { get; set; }
    }
}
