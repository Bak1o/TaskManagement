using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Models.Enums
{
    public enum UserStatus : byte
    {
        Inactive = 10,
        Active = 20,
        Suspended = 30,
        Closed = 99
    }
}
