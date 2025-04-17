using Microsoft.AspNetCore.Identity;
using TaskManagement.Identity.Models;

namespace TaskManagement.Identity.Services.Abstractions;

public interface ITokenService
{
    Task<string> GenerateAccessTokenForAsync(ApplicationUser user,UserManager<ApplicationUser> userManager);

    Task<string> GenerateRefreshTokenAsync(ApplicationUser user);

    Task<string> RefreshTokenAsync(string token);
}