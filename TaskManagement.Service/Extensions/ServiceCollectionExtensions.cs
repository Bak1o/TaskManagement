using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Abstractions;
using TaskManagement.Service.Services.Implementations;

namespace TaskManagement.Service.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            
            serviceCollection.AddScoped<IDomainTaskService, DomainTaskService>();
            serviceCollection.AddScoped<IProjectService, ProjectService>();
            return serviceCollection;

        }
    }
}
