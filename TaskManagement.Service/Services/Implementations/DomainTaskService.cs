using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Abstractions;
using TaskManagement.Domain.Commands;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models;
using TaskManagement.Service.Services.Abstractions;

namespace TaskManagement.Service.Services.Implementations
{
    public class DomainTaskService : IDomainTaskService
    {
        private readonly IDomainTaskRepository _repository;
        public DomainTaskService(IDomainTaskRepository repository)
        {
            _repository = repository;
        }
        public async Task<int> OpenAsync(DomainTask task)
        {
            task.Validate();
            task.Open();
            await _repository.CreateAsync(task);
            return task.Id;

        }

        public async Task UpdateAsync(UpdateTaskCommand command)
        {
            command.Validate();
            var taskExist = await _repository.GetByIdAsync(command.Id);
            

            command.UpdateMatchingCheck(taskExist);

            await _repository.UpdateAsync(taskExist);


        }
        public async Task CloseAsync(CloseTaskCommand command)
        {
            command.Validate();
            
            var taskexists = await _repository.GetByIdOrDefaultAsync(command.Id);
            
            taskexists.close();
            await _repository.UpdateAsync(taskexists);
            
        }
    }
}
