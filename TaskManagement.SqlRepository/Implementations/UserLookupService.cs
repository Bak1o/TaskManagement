using Microsoft.EntityFrameworkCore;
using TaskManagement.Service.DataTransferObjects;
using TaskManagement.Service.Services.Abstractions;
using TaskManagement.SqlRepository.Database;

namespace TaskManagement.SqlRepository.Implementations
{
    public sealed class UserLookupService : IUserLookupService
    {
        private readonly AppDbContext _dbContext;

        public UserLookupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserLookupDto?> FindByEmailAsync(string email)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Where(user => user.Email == email)
                .Select(user => new UserLookupDto
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty
                })
                .FirstOrDefaultAsync();
        }
    }
}
