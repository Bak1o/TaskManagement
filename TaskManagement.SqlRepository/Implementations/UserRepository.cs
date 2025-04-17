using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Identity.DataTransferObjects;
using TaskManagement.Identity.Exceptions;
using TaskManagement.Identity.Models;
using TaskManagement.Identity.Queries;
using TaskManagement.Identity.Services.Abstractions;
using TaskManagement.SqlRepository.Database;

namespace TaskManagement.SqlRepository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
       
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            
            
        }

        public async Task<List<ApplicationUserDto>> GetUsersByRoleAsync(string roleQuery)
        {
            if (roleQuery == null)
            {
                throw new ArgumentNullException(nameof(roleQuery));
            }
            

            var query = from user in _dbContext.Users
                            join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                            join role in _dbContext.Roles on userRole.RoleId equals role.Id
                            where role.Name == roleQuery
                            select new ApplicationUserDto
                            {
                                Id = user.Id,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Email = user.Email
                            };
            var users = await query.ToListAsync();

            if (users.Count == 0)
            {
                throw new NotFoundException($"No users found with role '{roleQuery}'.");
            }

            return users;



        }


    }
}
