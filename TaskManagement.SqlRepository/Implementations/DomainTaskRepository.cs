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

namespace TaskManagement.SqlRepository.Implementations
{
    public class DomainTaskRepository : IDomainTaskRepository
    {
        private readonly AppDbContext _dbContext;
       
        public DomainTaskRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            
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
            var task = await _dbContext.DomainTasks
                        .AsNoTracking()
                        .FirstOrDefaultAsync(t => t.Id == id)
                        ?? throw new ObjectNotFoundException(id.ToString(), nameof(DomainTask));
            var projectName = await _dbContext.Projects
            .Where(p => p.Id == task.ProjectId)
            .Select(p => p.Name)
            .FirstOrDefaultAsync();

            var userEmail = await _dbContext.Users
                .Where(u => u.Id == task.CreatedByUserId)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();
            return new TaskDto
            {
                Title = task.Title,
                Description = task.Description,
                ProjectId = task.ProjectId,
                ProjectName = projectName ?? string.Empty,
                Status = task.Status.ToString(),
                Priority = task.Priority.ToString(),
                StartDate = task.StartDate,
                Deadline = task.DeadLine,
                CreatedByUserEmail = userEmail ?? string.Empty,
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
            var task = await _dbContext.DomainTasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new ObjectNotFoundException(id.ToString(), nameof(DomainTask));

            var assignedUsers = await (
                from taskUser in _dbContext.TaskUsers
                join user in _dbContext.Users
                    on taskUser.ApplicationUserId equals user.Id
                where taskUser.DomainTaskId == id
                select new AssignedUsersDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email ?? string.Empty
                })
                .ToListAsync();

            return new TaskWithUsersDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                AssignedUsers = assignedUsers
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
                var userId = await _dbContext.Users
                        .Where(u => u.Email == filter.AssignedUserMail)
                        .Select(u => u.Id)
                        .FirstOrDefaultAsync();

                if (userId is null)
                {
                    throw new ObjectNotFoundException(filter.AssignedUserMail, "User");
                }

                query = query.Where(t =>
                    _dbContext.TaskUsers.Any(tu =>
                        tu.DomainTaskId == t.Id &&
                        tu.ApplicationUserId == userId));
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
            var result = await (
      from task in query
      join project in _dbContext.Projects
          on task.ProjectId equals project.Id
      join user in _dbContext.Users
          on task.CreatedByUserId equals user.Id
      select new TaskDto
      {
          Title = task.Title,
          Description = task.Description,
          ProjectId = task.ProjectId,
          ProjectName = project.Name,
          Status = task.Status.ToString(),
          Priority = task.Priority.ToString(),
          StartDate = task.StartDate,
          Deadline = task.DeadLine,
          CreatedByUserEmail = user.Email ?? string.Empty
      })
      .ToListAsync();
            // **4. Validate Result**
            if (!result.Any())
            {
                throw new ValidationException("No tasks found matching the given filters.");
            }

            return result;
        }


        public async Task SaveAsync(DomainTask task)
        {
            await _dbContext.SaveChangesAsync();
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
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user is null)
            {
                throw new ObjectNotFoundException(userEmail, "User");
            }

            _ = await GetByIdAsync(taskId);

            var alreadyAssigned = await _dbContext.TaskUsers.AnyAsync(tu =>
                tu.DomainTaskId == taskId &&
                tu.ApplicationUserId == user.Id);

            if (alreadyAssigned)
            {
                throw new ValidationException("User is already assigned to this task");
            }

            var taskUser = new TaskUser
            {
                DomainTaskId = taskId,
                ApplicationUserId = user.Id
            };

            _dbContext.TaskUsers.Add(taskUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveUserAsync(string userEmail, int taskId)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user is null)
            {
                throw new ObjectNotFoundException(userEmail, "User");
            }

            var assignedUsersCount = await _dbContext.TaskUsers
                .CountAsync(tu => tu.DomainTaskId == taskId);

            if (assignedUsersCount < 2)
            {
                throw new ValidationException("You can't remove last assigned user from task");
            }

            var taskUser = await _dbContext.TaskUsers
                .FirstOrDefaultAsync(tu =>
                    tu.DomainTaskId == taskId &&
                    tu.ApplicationUserId == user.Id);

            if (taskUser is null)
            {
                throw new ObjectNotFoundException(user.Id, nameof(TaskUser));
            }

            _dbContext.TaskUsers.Remove(taskUser);
            await _dbContext.SaveChangesAsync();


        }
        
    }
}
