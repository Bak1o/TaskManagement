using TaskManagement.Identity.Exceptions;
using TaskManagement.Identity.Models;
using TaskManagement.Identity.Requests;
using TaskManagement.Identity.Services.Abstractions;
using TaskManagement.MessageSender.Abstractions.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using IEmailSender = TaskManagement.MessageSender.Abstractions.Services.IEmailSender;
using TaskManagement.Identity.Responses;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Identity.DataTransferObjects;
using InvalidOperationException = TaskManagement.Identity.Exceptions.InvalidOperationException;
using TaskManagement.Identity.Queries;
using TaskManagement.Domain.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Identity.Services.Implementations;

public sealed class IdentityService : IIdentityService
{
   

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<IdentityService> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
   






    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IEmailSender emailSender,
        ILogger<IdentityService> logger, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _emailSender = emailSender;
        _logger = logger;
        _roleManager = roleManager;
    }

   
    

    public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new AuthenticationException();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (!result.Succeeded)
        {
            _logger.LogWarning("login failed. result: {@Result}", result);
            throw new AuthenticationException();
        }

        return await CreateLoginResponseAsync(user);
    }

    public async Task<LoginResponse?> RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new IdentityException(result.Errors);
        }
        if (!await _roleManager.RoleExistsAsync(request.Role))
        {
            throw new NotFoundException("Role not found");

        }
        var requestRole = request.Role.ToLower();
        if (requestRole == "admin" && _userManager.Users.Count() > 1)
        {
            throw new ValidationException("Admin role is forbidden");
        }

        await _userManager.AddToRoleAsync(user, request.Role);

        if (_userManager.Options.SignIn.RequireConfirmedAccount)
        {
                var code = _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                await SendOtpAsync(request.Email, code.Result);
                return null;
         }

        return await CreateLoginResponseAsync(user);
        

       
    }

    public async Task ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new AuthenticationException();
        }

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", request.Otp);
        if (!isValid)
        {
            throw new AuthenticationException();
        }

        user.EmailConfirmed = true;
        user.Activate();
        await _userManager.UpdateAsync(user);
    }

    public async Task<LoginResponse> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new AuthenticationException();
        }

        if (!await _userManager.CheckPasswordAsync(user, request.CurrentPassword))
        {
            throw new AuthenticationException();
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (result.Succeeded)
        {
            return await CreateLoginResponseAsync(user);
        }

        throw new IdentityException(result.Errors);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new AuthenticationException();
        }

        var otp = await _userManager.GenerateTwoFactorTokenAsync(user, "ResetPassword");
        await SendOtpAsync(request.Email, otp);
    }

    public async Task<LoginResponse> NewPasswordAsync(NewPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new AuthenticationException();
        }

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "ResetPassword", request.Otp);
        if (!isValid)
        {
            throw new AuthenticationException();
        }

        if (!string.IsNullOrEmpty(user.PasswordHash))
        {
            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                throw new ChangePasswordException(removePasswordResult.Errors, "Failed to remove the old password");
            }
        }

        var addPasswordResult = await _userManager.AddPasswordAsync(user, request.NewPassword);
        if (!addPasswordResult.Succeeded)
        {
            throw new ChangePasswordException(addPasswordResult.Errors, "Failed to remove the old password");
        }

        return await CreateLoginResponseAsync(user);
    }

    public async Task<ApplicationUserWithRolesDto> GetUserByEmailAsync(string email)
    {
       var user = await _userManager.FindByEmailAsync(email)
            ?? throw new NotFoundException($" user with Email : {email} was not found");
        var userRoles = await _userManager.GetRolesAsync(user);
       
        return new ApplicationUserWithRolesDto
        { 
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = email,
            Role = userRoles.ToList(),
        };
    }

    public async Task<List<ApplicationUserDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
                                
        if (users.Count == 0)
        {
            throw new NotFoundException("No users were found ");
        }
       return users
            .Select(user => new ApplicationUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email

            })
             .ToList();
        
    }
    public async Task<List<ApplicationUserDto>> GetUsersByRoleAsync(string role)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(role);

        if (usersInRole.Count == 0)
        {
            throw new NotFoundException($" users with role : {role} was not found ");
        }
            
         var users = usersInRole
            .Select(u => new ApplicationUserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email
            })
            .ToList();

        return users;
    }

    public async Task GiveAdminAsync(string email)
    {
       var user = await _userManager.FindByEmailAsync(email);
       
        if (user == null)
        {
            throw new NotFoundException($" user with Email : {email} was not found ");
        }
        var isInRole = await _userManager.IsInRoleAsync(user, "Admin");
       
        if(isInRole)
        {
            throw new InvalidOperationException($"User with Email: {email} is already an Admin");
        }

        var result = await _userManager.AddToRoleAsync(user, "Admin");
       
        if (!result.Succeeded)
            throw new InvalidOperationException("Failed to assign Admin role to the user.");

    }

    public async Task RemoveAdminAsync(string email)
    {
        var adminUser = await _userManager.FindByEmailAsync(email);
        if (adminUser == null)
        {
            throw new AuthenticationException();
        }
       
        var isInAdminStatus = await _userManager.IsInRoleAsync(adminUser, "admin");

        if (!isInAdminStatus)
        {
            throw new InvalidOperationException("User doesn't have Admin status");
        }

        var removeResult = await _userManager.RemoveFromRoleAsync(adminUser, "Admin");
        if (!removeResult.Succeeded)
            throw new InvalidOperationException("Failed to remove user from Admin role.");

        var updateResult = await _userManager.UpdateAsync(adminUser);
        if (!updateResult.Succeeded)
            throw new InvalidOperationException("Failed to update user after role removal.");

    }
    public async Task BanUserAsync(string email)
    {
       var user = await _userManager.FindByEmailAsync(email);
       
        if(user == null)
        {
            throw new NotFoundException($"User with Email: {email} was not found");
        }
        user.Status = Models.Enums.Status.Banned;
        await _userManager.UpdateAsync(user);

    }
    public async Task<List<ApplicationUserDto>> GetUsersAsync(UsersQueryFilter filter)
    {
        if (filter == null)
        {
            throw new ArgumentNullException(nameof(filter));
        }
        var query = _userManager.Users
                                .AsQueryable()
                                .AsNoTracking();
        
        if (filter.UserStatus.HasValue)
        {
            query = query.Where(u => u.Status == filter.UserStatus.Value);
        }

        if (filter.RegisteredDate.HasValue)
        {
            var date = filter.RegisteredDate.Value.ToDateTime(TimeOnly.MinValue);
            var nextDate = date.AddDays(1);

            query = query.Where(u => u.RegisteredAt >= date && u.RegisteredAt < nextDate);
        }

        if (filter.SortByRegisteredDate && !filter.SortDescending)
        {
            query = query.OrderBy(u => u.RegisteredAt);
        }

        if (filter.SortByRegisteredDate && filter.SortDescending)
        {
            query = query.OrderByDescending(u => u.RegisteredAt);
        }

        var result = await query.Select(u => new ApplicationUserDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email
        }).ToListAsync();
       
        if (!result.Any())
        {
            throw new ValidationException("No tasks found matching the given filters.");
        }

        return result;


    }

    

    private async Task<LoginResponse> CreateLoginResponseAsync(ApplicationUser user)
    {
        return new LoginResponse
        {
            AccessToken = await _tokenService.GenerateAccessTokenForAsync(user,_userManager),
            RefreshToken = await _tokenService.GenerateRefreshTokenAsync(user),
            UserId = user.Id
        };
        
    }

    private async Task SendOtpAsync(string email, string otp)
    {
        await _emailSender.SendEmailAsync(email, subject: "Otp", otp);
    }

   
}