using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models;
using TaskManagement.Service.Services.Abstractions;
using TaskManagement.SqlRepository.Database;

namespace TaskManagement.SqlRepository.Implementations
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _dbContext;
        public ProjectRepository(AppDbContext dbContext )
        {
            _dbContext = dbContext;
        }
        public async Task<int> CreateAsync(Project projectToCreate)
        {
            await  _dbContext.Projects.AddAsync( projectToCreate );
            
            await _dbContext.SaveChangesAsync();
            return projectToCreate.Id;
            
        }

        public async Task DeleteAsync(int id)
        {
            var project = await GetByIdAsync(id);

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            return await GetByIdOrDefaultAsync( id )?? throw new ObjectNotFoundException(id.ToString(), nameof(Project));
        }

        public async Task<Project> GetByIdOrDefaultAsync(int id)
        {
         return await  _dbContext.Projects.FindAsync( id );
            
        }

        public Task<List<Project>> ListAsync()
        {
            return _dbContext.Projects
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task SaveAsync(Project project)
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project projectToUpdate)
        {
             _dbContext.Projects.Attach( projectToUpdate );
            await SaveAsync( projectToUpdate );
            
        }
    }
}
