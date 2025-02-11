using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;

namespace TaskManagement.Service.Services.Abstractions
{
    public interface IDomainTaskRepository
    {
        Task<int> CreateAsync(DomainTask taskToCreate);
        Task<List<DomainTask>> ListAsync();
        Task<DomainTask> GetByIdAsync(int id);
        Task<DomainTask> GetByIdOrDefaultAsync(int id);

        Task UpdateAsync(DomainTask taskToUpdate);
        Task DeleteAsync(int id);
        Task SaveAsync(DomainTask task);
    }
}
