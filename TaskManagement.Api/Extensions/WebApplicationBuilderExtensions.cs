using TaskManagement.FileRepository.Extensions;
using TaskManagement.Service.Extensions;
using TaskManagement.SqlRepository.Extensions;

namespace TaskManagement.Api.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddServices()
                .AddSqlRepositories();
            return builder;
        }
    }
}
