using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.FileRepository.Abstractions;
using TaskManagement.FileRepository.Implementations;
using TaskManagement.Service.Services.Abstractions;

namespace TaskManagement.FileRepository.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ICustomeRepository, FileCustomerRepository>();
            serviceCollection.AddScoped<IProjectRepository, FileProjectRepository>();
            serviceCollection.AddScoped<IDomainTaskRepository, FileDomainTaskRepository>();
            serviceCollection.AddScoped<ISequenceProvider,FileSequenceProvider>();
            return serviceCollection;
        }
    }
}
