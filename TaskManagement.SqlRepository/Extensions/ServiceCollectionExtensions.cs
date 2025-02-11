using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Service.Services.Abstractions;
using TaskManagement.SqlRepository.Implementations;

namespace TaskManagement.SqlRepository.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddSqlRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICustomeRepository, CustomerRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IDomainTaskRepository, DomainTaskRepository>();
            return services;
        }
    }
}
