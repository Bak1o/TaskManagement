using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Identity.Models.Enums;

namespace TaskManagement.Identity.Queries
{
    public sealed class UsersQueryFilter
    {
        //public string? UserRole { get; set; }
        public Status? UserStatus { get; set; }
        public DateOnly? RegisteredDate { get; set; }
        public bool SortByRegisteredDate { get; set; } = false;
        public bool SortDescending {  get; set; } = false ;

    }
}
