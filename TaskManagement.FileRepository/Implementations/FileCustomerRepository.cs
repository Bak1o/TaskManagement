using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.FileRepository.Abstractions;
using TaskManagement.FileRepository.Models;
using TaskManagement.Service.Services.Abstractions;

namespace TaskManagement.FileRepository.Implementations
{
    public class FileCustomerRepository : FileRepositoryBase<Customer, int>, ICustomeRepository
    {
        private readonly ISequenceProvider _sequenceProvider;
        public FileCustomerRepository(IOptions<FileStorageOptions> options, ISequenceProvider sequenceProvider) : base(options.Value.UserRepositoryPath)
        {
            _sequenceProvider = sequenceProvider;
        }
        #region comment section
        //public Task CreateUserAsync(User userToCreate)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task DeleteUserAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<User> GetByIdOrDefaultAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<User> GetUserAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<User> GetUserAsync(string name)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task SaveUserAsync(User user)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task UpdateUserAsync(UpdateUser updateUser)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<List<User>> UsersListAsync()
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
        protected override Task<int> GenerateIdAsync() =>
            _sequenceProvider.GetNextInteger("customers");
    }
}
