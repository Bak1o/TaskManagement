using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Service.DataTransferObjects;

namespace TaskManagement.Service.Services.Abstractions
{
    public interface IUserLookupService
    {
        Task<UserLookupDto?> FindByEmailAsync(string email);
    }
}
