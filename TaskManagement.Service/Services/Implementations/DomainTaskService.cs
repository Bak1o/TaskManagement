
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Abstractions;
using TaskManagement.Domain.Commands;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Models.Enums;

using TaskManagement.Service.DataTransferObjects;
using TaskManagement.Service.Services.Abstractions;

namespace TaskManagement.Service.Services.Implementations
{
    public class DomainTaskService : IDomainTaskService
    {
        private readonly IDomainTaskRepository _repository;
        private readonly IUserLookupService _userLookupService;
        public DomainTaskService(IDomainTaskRepository repository, IUserLookupService userLookupService)
        {
            _repository = repository;
            _userLookupService = userLookupService;

        }
        public async Task<int> OpenAsync(OpenTaskCommand command)
        {
            command.Validate();
            var assignedUsers = new List<UserLookupDto>();
            foreach (var email in command.AssignedUsersEmails)
            {
                var user = await _userLookupService.FindByEmailAsync(email);
                if (user != null)
                {
                    assignedUsers.Add(user);
                }
            }

            if (!assignedUsers.Any())
            {
                throw new ValidationException("No valid users found for assignment.");

            }

            // Step 2: Find the Creator User using UserManager
            var creatorUser = await _userLookupService.FindByEmailAsync(command.CreatedByUserMail);
            if (creatorUser == null)
            {
                throw new ObjectNotFoundException(command.CreatedByUserMail, "User");
            }

            // Step 3: Create the DomainTask
            var task = new DomainTask
            {
                Title = command.Title,
                Description = command.Description,
                ProjectId = command.ProjectId,
                Priority = command.Priority,
                DeadLine = command.DeadLine,
                CreatedByUserId = creatorUser.Id,
               
                Status = Status.ToDo
            };

            // Step 4: Create TaskUser Entries
            var taskUsers = assignedUsers.Select(user => new TaskUser
            {
                DomainTask = task,
                DomainTaskId = task.Id,
                
                ApplicationUserId = user.Id
            }).ToList();

            await _repository.CreateAsync(task,taskUsers);
            return task.Id;

        }

        public async Task UpdateAsync(UpdateTaskCommand command)
        {
            command.Validate();
            var taskExist = await _repository.GetByIdAsync(command.Id);
            

            command.UpdateMatchingCheck(taskExist);

            

                 
            

            await _repository.UpdateAsync(taskExist);


        }

        public async Task AssignUserAsync(string userEmail, int id)
        {
          await _repository.AddUserAsync(userEmail, id);
        }

        public async Task RemoveAssignedUserAsync(string userEmail, int id)
        {
            await _repository.RemoveUserAsync(userEmail, id);


        }
        public async Task CloseAsync(CloseTaskCommand command)
        {
            command.Validate();
            
            var taskexists = await _repository.GetByIdAsync(command.Id);
            
            taskexists.close();
            await _repository.UpdateAsync(taskexists);
            
        }
    }
}
