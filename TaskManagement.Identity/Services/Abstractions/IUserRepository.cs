using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Identity.DataTransferObjects;

namespace TaskManagement.Identity.Services.Abstractions
{
    public interface IUserRepository
    {
        Task<List<ApplicationUserDto>> GetUsersByRoleAsync(string roleQuery);
    }
}
