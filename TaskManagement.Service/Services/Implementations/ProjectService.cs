
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Abstractions;
using TaskManagement.Domain.Commands;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models;

using TaskManagement.Service.Services.Abstractions;

namespace TaskManagement.Service.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserLookupService _userLookupService;


        public ProjectService(IProjectRepository projectRepository, IUserLookupService userLookupService)
        {
            _projectRepository = projectRepository;
            _userLookupService = userLookupService;

        }



        //public bool ValidateCreateProject(Project project)
        //{
        //    if (_inMemoryDb.Projects.Count > 0)
        //    {
        //        if (_inMemoryDb.Projects.Any(p => p.Id == project.Id))
        //            throw new OwnValidationException(" project Id already exists ");
        //    }

        //    if (project.Name.Length < 1 || project.Name.Length > 100)
        //        throw new OwnValidationException(" project name must contain minimum 1 symbol and maximum 100 symbol ");

        //    if (project.Description.Length is < 1 or > 4000)
        //        throw new OwnValidationException(" project description must contain minimum 1 symbol and maximum 4000 symbol ");

        //    if (_inMemoryDb.Users.TrueForAll(u => u.Id != project.CreatedByUserId))
        //        throw new OwnValidationException($" user with this Id {project.CreatedByUserId} is not present in user base ");


        //    return true;
        //}

        public async Task<int> ExecuteAsync(RegisterProjectCommand command)
        {
            command.Validate();
            var user = await _userLookupService.FindByEmailAsync(command.CreatedByUserMail);
            if (user == null)
            {
                throw new ObjectNotFoundException(command.CreatedByUserMail,nameof(user));
            }
            var project = new Project
                 (command.Name,
                  command.Description,
                  user.Id);
            await _projectRepository.CreateAsync(project);
            return project.Id;

        }
        public async Task ExecuteAsync(OpenProjectCommand command)
        {
            command.Validate();
            var projectExists = await _projectRepository.GetByIdAsync(command.Id);
           

            projectExists.Open(command.EndDate);
            await _projectRepository.UpdateAsync(projectExists);
        }
        public async Task ExecuteAsync(CloseProjectCommand command)
        {
            command.Validate();
            var projectExists = await _projectRepository.GetByIdAsync(command.Id);
           
            projectExists.Close();
            await _projectRepository.UpdateAsync(projectExists);
        }

        public async Task ExecuteAsync(SuspendProjectCommand command)
        {
            command.Validate();
            var projectExists = await _projectRepository.GetByIdAsync(command.Id);
           
            projectExists.Suspend();
            await _projectRepository.UpdateAsync(projectExists);










        }
    }
}
