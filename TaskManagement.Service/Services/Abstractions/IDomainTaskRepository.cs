using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Queries;
using TaskManagement.Identity.Models;
using TaskManagement.Service.DataTransferObjects;
using TaskManagement.SqlRepository.DataTransferObjects;

namespace TaskManagement.Service.Services.Abstractions
{
    public interface IDomainTaskRepository
    {
        Task<int> CreateAsync(DomainTask taskToCreate,List<TaskUser> taskUsers);
        Task<List<TaskDto>> ListAsync(TaskQueryFilter filter);
        Task<DomainTask> GetByIdAsync(int id);
        Task<TaskDto> GetByIdDtoAsync(int id);
        Task<TaskWithUsersDto> GetByIdWithUsersAsync(int id);
        Task<DomainTask> GetByIdOrDefaultAsync(int id);

        Task UpdateAsync(DomainTask taskToUpdate);
        Task AddUserAsync(string userEmail, int taskId);
        Task RemoveUserAsync(string userEmail, int taskId);
        Task DeleteAsync(int id);
        Task SaveAsync(DomainTask task);
    }
}
