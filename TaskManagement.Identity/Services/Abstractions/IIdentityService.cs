using TaskManagement.Identity.DataTransferObjects;
using TaskManagement.Identity.Models;
using TaskManagement.Identity.Queries;
using TaskManagement.Identity.Requests;
using TaskManagement.Identity.Responses;

namespace TaskManagement.Identity.Services.Abstractions;

public interface IIdentityService
{
    Task<LoginResponse> AuthenticateAsync(LoginRequest request);
    Task<LoginResponse?> RegisterAsync(RegisterRequest request);
    Task ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<LoginResponse> ChangePasswordAsync(ChangePasswordRequest request);
    Task ResetPasswordAsync(ResetPasswordRequest request);
    Task<LoginResponse> NewPasswordAsync(NewPasswordRequest request);
    Task<ApplicationUserWithRolesDto> GetUserByEmailAsync(string email);
    Task<List<ApplicationUserDto>> GetAllUsersAsync();
    Task<List<ApplicationUserDto>> GetUsersByRoleAsync(string role);
    Task GiveAdminAsync(string email);
    Task RemoveAdminAsync(string email);
    Task BanUserAsync(string email);
    Task<List<ApplicationUserDto>> GetUsersAsync(UsersQueryFilter filter);
}