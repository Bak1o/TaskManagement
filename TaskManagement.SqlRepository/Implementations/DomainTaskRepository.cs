using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Service.Services.Abstractions;

namespace TaskManagement.SqlRepository.Implementations
{
    public class DomainTaskRepository : IDomainTaskRepository
    {
        public Task CreateAsync(DomainTask taskToCreate)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DomainTask> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DomainTask> GetByIdOrDefaultAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DomainTask>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(DomainTask task)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(DomainTask taskToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
