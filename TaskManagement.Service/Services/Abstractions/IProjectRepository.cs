using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;

namespace TaskManagement.Service.Services.Abstractions
{
    public interface IProjectRepository
    {
        Task<int> CreateAsync(Project projectToCreate);
        Task<List<Project>> ListAsync();
        Task<Project> GetByIdAsync(int id);
        Task<Project> GetByIdOrDefaultAsync(int id);

        Task UpdateAsync(Project projectToUpdate);
        Task DeleteAsync(int id);
        Task SaveAsync(Project project);
    }
}
