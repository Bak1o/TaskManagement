using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Service.Services.Abstractions
{
    public interface IRepository<TRepo, TUpdateModel>
       where TRepo : class
       where TUpdateModel : class
    {
        Task CreateAsync(TRepo objectToCreate);
        Task<List<TRepo>> ListAsync();
        Task<TRepo> GetByIdAsync(int id);
        Task<TRepo> GetByIdOrDefaultAsync(int id);

        Task UpdateAsync(TUpdateModel update);
        Task DeleteAsync(int id);
        Task SaveAsync(TRepo model);
    }
}
