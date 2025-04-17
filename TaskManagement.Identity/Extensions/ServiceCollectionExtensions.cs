using TaskManagement.Identity.Models;
using TaskManagement.Identity.Services.Abstractions;
using TaskManagement.Identity.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace TaskManagement.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IdentityBuilder AddIdentityServices(this IServiceCollection services,
        IConfiguration configuration, Action<IdentityOptions>? setupAction = null)
    {
        var identityBuilder =
            services
        .AddScoped<ITokenService, JwtTokenService>()
        .Configure<TokenServiceOptions>(configuration.GetSection(TokenServiceOptions.Key))
        .AddScoped<IIdentityService, IdentityService>()
        .AddIdentity<ApplicationUser, IdentityRole>(setupAction ?? DefaultSetupAction)
        //.AddIdentityCore<ApplicationUser>(setupAction ?? DefaultSetupAction)
        //.AddRoles<IdentityRole>()
        .AddTokenProvider<PasswordTokenProvider<ApplicationUser>>("ResetPassword")
        .AddDefaultTokenProviders();
        //services.AddScoped<SignInManager<ApplicationUser>>();



     
        return identityBuilder;
    }

private static void DefaultSetupAction(IdentityOptions options)
    {
        options.Password = new PasswordOptions
        {
            RequireDigit = true,
            RequireLowercase = true,
            RequireNonAlphanumeric = true,
            RequireUppercase = true
            
        };

        options.Lockout = new LockoutOptions { AllowedForNewUsers = true, DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3), MaxFailedAccessAttempts = 3, };

        options.SignIn.RequireConfirmedAccount = true;
        options.SignIn.RequireConfirmedEmail = true;
    }
}