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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;


        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;

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
            var project = new Project
                 (command.Name,
                  command.Description,
                  command.CreatedByUserId);
            await _projectRepository.CreateAsync(project);
            return project.Id;

        }
        public async Task ExecuteAsync(OpenProjectCommand command)
        {
            command.Validate();
            var projectExists = await _projectRepository.GetByIdOrDefaultAsync(command.Id);
           

            projectExists.Open(command.EndDate);
            await _projectRepository.UpdateAsync(projectExists);
        }
        public async Task ExecuteAsync(CloseProjectCommand command)
        {
            command.Validate();
            var projectExists = await _projectRepository.GetByIdOrDefaultAsync(command.Id);
           
            projectExists.Close();
            await _projectRepository.UpdateAsync(projectExists);
        }

        public async Task ExecuteAsync(SuspendProjectCommand command)
        {
            command.Validate();
            var projectExists = await _projectRepository.GetByIdOrDefaultAsync(command.Id);
           
            projectExists.Suspend();
            await _projectRepository.UpdateAsync(projectExists);







            //public Task CreateAsync(Project projectToCreate)
            //{
            //    if (ValidateCreateProject(projectToCreate))

            //    {
            //        if (projectToCreate.Id == 0)
            //        {
            //            projectToCreate.Id = _inMemoryDb.Projects.Count > 0 ? _inMemoryDb.Projects.Max(u => u.Id) + 1 : 1;
            //        }

            //        _inMemoryDb.Projects.Add(projectToCreate);
            //    }
            //    return Task.CompletedTask;
            //}

            //public Task<List<Project>> ListAsync()
            //{
            //    return Task.FromResult(_inMemoryDb.Projects);
            //}

            //public Task<Project> GetByIdAsync(int id)
            //{
            //    return Task.FromResult( _inMemoryDb.Projects.FirstOrDefault(p => p.Id == id)!);
            //}

            //public Task<Project> GetByIdOrDefaultAsync(int id)
            //{
            //    return Task.FromResult(_inMemoryDb.Projects.FirstOrDefault(p => p.Id == id)!);
            //}

            //public async Task UpdateAsync(UpdateProject updateProject)
            //{
            //    var requestedProjectExist =  _inMemoryDb.Projects.Find(u => u.Id == updateProject.Id);
            //    if (requestedProjectExist == null)
            //    {
            //        throw new OwnValidationException($" Project with id = {updateProject.Id} doesn't exists");
            //    }

            //    updateProject.Validate();

            //        ProjectTransform.TransformFromModelToRepositoryModel(updateProject,requestedProjectExist);

            //}

            //public async Task DeleteAsync(int id)
            //{
            //    var project = _inMemoryDb.Projects.FirstOrDefault(p => p.Id == id);
            //    if (project == null)
            //    {
            //        throw new OwnValidationException($" Project with id = {id} doesn't exists");
            //    }
            //    _inMemoryDb.Projects.Remove(project);
            //}
            //public  Task SaveAsync(Project project)
            //{
            //    return Task.CompletedTask;
            //}



        }
    }
}
