using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Identity.Services.Abstractions;
using TaskManagement.Service.Services.Abstractions;
using TaskManagement.SqlRepository.Implementations;

namespace TaskManagement.SqlRepository.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddSqlRepositories(this IServiceCollection services)
        {
            
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IDomainTaskRepository, DomainTaskRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
