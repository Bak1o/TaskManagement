using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Identity.Models.Enums
{
    public enum Status : byte
    {
        Active,
        Inactive,
        Suspended,
        Banned
    }
}
