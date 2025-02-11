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
    public class FileProjectRepository : FileRepositoryBase<Project, int>, IProjectRepository
    {
        private readonly ISequenceProvider _sequenceProvider;
        public FileProjectRepository(IOptions<FileStorageOptions> options, ISequenceProvider sequenceProvider) : base(options.Value.ProjectRepositoryPath)//@"C:\Users\Admin\source\repos
                                                                                                                                                          //\TaskManagement.Service\TaskManagement.Service\DataFile\ProjectsData.json")
        {
            _sequenceProvider = sequenceProvider;
        }
        //public void CreateProject(Project projectToCreate)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Project> GetAllProjects()
        //{
        //    throw new NotImplementedException();
        //}

        //public Project GetProject(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UpdateProject(UpdateProject updateProject)
        //{
        //    throw new NotImplementedException();
        //}

        protected override Task<int> GenerateIdAsync() =>
          _sequenceProvider.GetNextInteger("projects");
    }
}
