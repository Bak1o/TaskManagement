using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Service.Services.Abstractions;

namespace TaskManagement.SqlRepository.Implementations
{
    public class ProjectRepository : IProjectRepository
    {
        public Task CreateAsync(Project projectToCreate)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetByIdOrDefaultAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(Project project)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Project projectToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
