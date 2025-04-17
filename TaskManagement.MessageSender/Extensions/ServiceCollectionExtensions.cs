using TaskManagement.MessageSender.Abstractions.Models;
using TaskManagement.MessageSender.Abstractions.Services;
using TaskManagement.MessageSender.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManagement.MessageSender.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMailSender(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailSender, EmailSender>();
        services.Configure<EmailSenderOptions>(configuration.GetSection(EmailSenderOptions.Key));

        return services;
    }
}