using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;

namespace TaskManagement.Service.Services.Abstractions
{
    public interface ICustomeRepository
    {
        Task<int> CreateAsync(Customer userToCreate);
        Task<List<Customer>> ListAsync();
        Task<Customer> GetByIdAsync(int id);
        Task<Customer> GetByIdOrDefaultAsync(int id);

        Task UpdateAsync(Customer userToUpdate);
        Task DeleteAsync(int id);
        Task SaveAsync(Customer user);
    }
}
