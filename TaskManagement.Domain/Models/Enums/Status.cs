using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Models.Enums
{
    public enum Status : byte
    { ToDo = 0, InProgress = 1, Done = 2, Suspended = 3 }
}
