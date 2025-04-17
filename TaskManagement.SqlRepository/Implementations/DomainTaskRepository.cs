using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Commands;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Models.Enums;
using TaskManagement.Domain.Queries;
using TaskManagement.Identity.Exceptions;
using TaskManagement.Identity.Models;
using TaskManagement.Service.DataTransferObjects;
using TaskManagement.Service.Services.Abstractions;
using TaskManagement.SqlRepository.Database;
using TaskManagement.SqlRepository.DataTransferObjects;

namespace TaskManagement.SqlRepository.Implementations
{
    public class DomainTaskRepository : IDomainTaskRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public DomainTaskRepository(AppDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task<int> CreateAsync(DomainTask taskToCreate, List<TaskUser> taskUsers)
        {

            await _dbContext.DomainTasks.AddAsync(taskToCreate);
            await _dbContext.TaskUsers.AddRangeAsync(taskUsers);
            await _dbContext.SaveChangesAsync();
            return taskToCreate.Id;

           
        }

        public async Task DeleteAsync(int id)
        {
           
            var task = await GetByIdAsync(id);
            _dbContext.DomainTasks.Remove(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TaskDto> GetByIdDtoAsync(int id)
        {
            var task = await GetByIdOrDefaultAsync(id) ?? throw new ObjectNotFoundException(id.ToString(), nameof(DomainTask));
           var project = await _dbContext.Projects.FindAsync(task.ProjectId);
            var user = await _userManager.FindByIdAsync(task.CreatedByUserId);
            return new TaskDto
            {
                Title = task.Title,
                Description = task.Description,
                ProjectId = task.ProjectId,
                ProjectName = project.Name,
                Status = task.Status.ToString(),
                Priority = task.Priority.ToString(),
                StartDate = task.StartDate,
                Deadline = task.DeadLine,
                CreatedByUserEmail = user.Email
            };
        }

        public async Task<DomainTask> GetByIdAsync(int id)
        {
            return await GetByIdOrDefaultAsync(id) ?? throw new ObjectNotFoundException(id.ToString(), nameof(DomainTask));
        }

        public async Task<DomainTask> GetByIdOrDefaultAsync(int id)
        {
            return await _dbContext.DomainTasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskWithUsersDto> GetByIdWithUsersAsync(int id)
        {
            var taskWithUsers = await _dbContext.DomainTasks
                               .Include(t => t.AssignedUsers) // Include the TaskUser relationship
                               .ThenInclude(tu => tu.ApplicationUser) // Include the ApplicationUser from TaskUser
                               .FirstOrDefaultAsync(t => t.Id == id);

            if (taskWithUsers == null)
            {
                throw new ObjectNotFoundException(id.ToString(), nameof(DomainTask));
            }
            return new TaskWithUsersDto
            {
                Id = taskWithUsers.Id,
                Title = taskWithUsers.Title,
                Description = taskWithUsers.Description,
                AssignedUsers = taskWithUsers.AssignedUsers.Select(au => new AssignedUsersDto
                {
                    FirstName = au.ApplicationUser.FirstName,
                    LastName = au.ApplicationUser.LastName,
                    Email = au.ApplicationUser.Email ?? string.Empty
                }).ToList()

            };
        }

        //public async Task<List<DomainTask>> ListAsync(TaskQueryFilter filter)
        //{
        //    if (filter == null) throw new ArgumentNullException(nameof(filter));

        //    var query = _dbContext.DomainTasks.AsQueryable();

        //    // **1. Filtering**
        //    if (!string.IsNullOrWhiteSpace(filter.Title))
        //    {
        //        query = query.Where(t => t.Title == filter.Title);
        //    }

        //    if (filter.Status.HasValue)
        //    {
        //        query = query.Where(t => t.Status == filter.Status.Value);
        //    }

        //    if (!string.IsNullOrWhiteSpace(filter.AssignedUserMail))
        //    {
        //        query = query.Where(t => t.AssignedUsers.Any(u => u.ApplicationUser.Email == filter.AssignedUserMail));
        //    }

        //    if (filter.Deadline.HasValue)
        //    {
        //        query = query.Where(t => t.DeadLine == filter.Deadline.Value);
        //    }

        //    if (filter.DoneDate.HasValue)
        //    {
        //        query = query.Where(t => t.Status == Status.Done && t.DeadLine == filter.DoneDate.Value);
        //    }

        //    // **2. Sorting**
        //    if (!string.IsNullOrWhiteSpace(filter.SortBy))
        //    {
        //        query = filter.SortBy.ToLower() switch
        //        {
        //            "priority" => query.OrderBy(t => t.Priority),
        //            "deadline" => query.OrderBy(t => t.DeadLine),
        //            _ => throw new ArgumentOutOfRangeException(nameof(filter.SortBy), filter.SortBy, null)
        //        };
        //    }

        //    // **Final Return Statement to Ensure Code Completes**
        //    var result = await query.ToListAsync();
        //    if (!result.Any())
        //    {
        //        throw new ValidationException("No tasks found matching the given filters.");
        //    }
        //    return result;



        //}

        public async Task<List<TaskDto>> ListAsync(TaskQueryFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            var query = _dbContext.DomainTasks.AsQueryable();

            // **1. Filtering**
            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                query = query.Where(t => t.Title == filter.Title);
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(t => t.Status == filter.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.AssignedUserMail))
            {
                query = query.Where(t => t.AssignedUsers.Any(u => u.ApplicationUser.Email == filter.AssignedUserMail));
            }

            if (filter.Deadline.HasValue)
            {
                query = query.Where(t => t.DeadLine == filter.Deadline.Value);
            }

            if (filter.DoneDate.HasValue)
            {
                query = query.Where(t => t.Status == Status.Done && t.DeadLine == filter.DoneDate.Value);
            }

            // **2. Sorting**
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                query = filter.SortBy.ToLower() switch
                {
                    "priority" => query.OrderBy(t => t.Priority),
                    "deadline" => query.OrderBy(t => t.DeadLine),
                    _ => throw new ArgumentOutOfRangeException(nameof(filter.SortBy), filter.SortBy, null)
                };
            }

            // **3. Project to TaskDto**
            var result = await query.Select(t => new TaskDto
            {
                Title = t.Title,
                Description = t.Description,
                ProjectId = t.Project.Id,  // Assuming Project is a navigation property
                ProjectName = t.Project.Name, // Assuming Project has a Name property
                Status = t.Status.ToString(), // Convert Enum to string
                Priority = t.Priority.ToString(), // Convert Enum or int to string
                StartDate = t.StartDate,
                Deadline = t.DeadLine,
                CreatedByUserEmail = t.CreatedByUser.Email // Assuming CreatedBy is a user reference
            }).ToListAsync();

            // **4. Validate Result**
            if (!result.Any())
            {
                throw new ValidationException("No tasks found matching the given filters.");
            }

            return result;
        }


        public Task SaveAsync(DomainTask task)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(DomainTask taskToUpdate)
        {
        //    if (command.AssignedUsersEMails != null)
        //    {
        //        var assignedUsers = new List<ApplicationUser>();
        //        foreach (var email in command.AssignedUsersEMails)
        //        {
        //            var user = await _userManager.FindByEmailAsync(email);
        //            if (user != null)
        //            {
        //                assignedUsers.Add(user);
        //            }
        //        }

        //        if (!assignedUsers.Any())
        //        {
        //            throw new NotFoundException("No valid users found for assignment.");
        //        }

            _dbContext.Attach(taskToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddUserAsync(string userEmail, int taskId)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            var task = await GetByIdAsync(taskId);
            if (user == null)
            {
                throw new NotFoundException($" user with Email : {userEmail} was not found ");
            }
           
            var taskUser = new TaskUser
            {
                DomainTask = task,
                DomainTaskId = task.Id,
                ApplicationUser = user,
                ApplicationUserId = user.Id,
            };
            
             _dbContext.TaskUsers.Add(taskUser);
            task.AssignedUsers.Add(taskUser);
             _dbContext.Update(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveUserAsync(string userEmail, int taskId)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new NotFoundException($" user with Email : {userEmail} was not found ");
            }
            var task = await GetByIdAsync(taskId);
            
            
            var taskUser = await _dbContext.TaskUsers.FindAsync(user.Id);
            if (taskUser == null)
            {
                throw new NotFoundException($" no assign user was found with user Id : {user.Id}");
            }

            if (task.AssignedUsers.Count < 2)
            {
                throw new ValidationException(" You can't remove last assigned user from task ");
            }
            task.AssignedUsers.Remove(taskUser);
            _dbContext.Update(task);
            await _dbContext.SaveChangesAsync();
           
            
        }
        
    }
}
