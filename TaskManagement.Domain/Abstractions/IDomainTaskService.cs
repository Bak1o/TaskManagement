using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Commands;
using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.Abstractions
{
    public interface IDomainTaskService
    {

        Task<int> OpenAsync(OpenTaskCommand command);
        Task UpdateAsync(UpdateTaskCommand command);
        Task AssignUserAsync(string userEmail, int id);
        Task RemoveAssignedUserAsync(string userEmail, int id);
        Task CloseAsync(CloseTaskCommand command);

    }
}