using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models;
using TaskManagement.Service.Services.Abstractions;
using TaskManagement.SqlRepository.Database;

namespace TaskManagement.SqlRepository.Implementations
{
    public sealed class CustomerRepository : ICustomeRepository
    {
        private readonly AppDbContext _dbContext;
        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;   
        }
        public async Task<int> CreateAsync(Customer userToCreate)
        {
            await _dbContext.Customers.AddAsync(userToCreate);
           await _dbContext.SaveChangesAsync();
            return userToCreate.Id;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await GetByIdOrDefaultAsync(id);
            
        }

        public async Task<Customer> GetByIdOrDefaultAsync(int id)
        {
            return await _dbContext.Customers.FindAsync(id)?? throw new ObjectNotFoundException(id.ToString(), nameof(Customer));
            
        }

        public Task<List<Customer>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(Customer user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Customer userToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
