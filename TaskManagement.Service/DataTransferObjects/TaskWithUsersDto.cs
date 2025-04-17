using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Service.DataTransferObjects;

namespace TaskManagement.SqlRepository.DataTransferObjects
{
    public class TaskWithUsersDto
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required List<AssignedUsersDto> AssignedUsers { get; set; }
    }
}
