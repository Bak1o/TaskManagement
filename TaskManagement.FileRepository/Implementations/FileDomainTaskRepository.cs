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
    public class FileDomainTaskRepository : FileRepositoryBase<DomainTask, int>, IDomainTaskRepository
    {
        private readonly ISequenceProvider _sequenceProvider;
        public FileDomainTaskRepository(IOptions<FileStorageOptions> options, ISequenceProvider sequenceProvider) : base(options.Value.DomainTaskRepositoryPath)
        {
            _sequenceProvider = sequenceProvider;
        }
        //public void CreateDomainTask(DomainTask taskToCreate)
        //{
        //    throw new NotImplementedException();
        //}

        //public void DeleteDomainTask(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<DomainTask> GetAllTasks()
        //{
        //    throw new NotImplementedException();
        //}

        //public DomainTask GetDomainTask(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UpdateDomainTask(UpdateDomainTask updateTask)
        //{
        //    throw new NotImplementedException();
        //}
        protected override Task<int> GenerateIdAsync() =>
           _sequenceProvider.GetNextInteger("domaintasks");
    }
}
