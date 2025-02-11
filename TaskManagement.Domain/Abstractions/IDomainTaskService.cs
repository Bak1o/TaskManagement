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

        Task<int> OpenAsync(DomainTask task);
        Task UpdateAsync(UpdateTaskCommand command);
        Task CloseAsync(CloseTaskCommand command);

    }
}