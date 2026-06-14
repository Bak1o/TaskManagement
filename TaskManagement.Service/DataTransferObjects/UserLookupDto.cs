using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Service.DataTransferObjects
{
    public sealed class UserLookupDto
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
    }
}
