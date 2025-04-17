using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using TaskManagement.Domain.Models.Enums;
using TaskManagement.Identity.Extensions;
using TaskManagement.MessageSender.Extensions;
using TaskManagement.Service.Extensions;
using TaskManagement.SqlRepository.Database;
using TaskManagement.SqlRepository.Extensions;

namespace TaskManagement.Api.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddSwaggerDocumentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
                options.MapType<Status>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetNames(typeof(Status))
              .Select(name => (IOpenApiAny)new OpenApiString(name))
              .ToList()
                });

                options.MapType<Priority>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetNames(typeof(Priority))
                               .Select(name => (IOpenApiAny)new OpenApiString(name))
                               .ToList()
                });

                options.ExampleFilters(); // ამატებს მაგალითებს სვაგერი დოკუმენტაციაში
            });
            // არეგისტრირებს მაგალთის კლასებს სერვისის კოლექციაში
            builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

            return builder;
        }

        public static WebApplicationBuilder AddReloadableAppSettings(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            return builder;
        }

        public static WebApplicationBuilder AddJwtAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddJwtBearerAuthentication(builder.Configuration);
            return builder;
        }
        public static WebApplicationBuilder AddAuthorizationPolicies(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorizationPolicies();
            return builder;
        }
        public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                 .ReadFrom.Configuration(builder.Configuration)
                 .CreateLogger();
            builder.Host.UseSerilog();
            return builder;
        }

        public static WebApplicationBuilder AddIdentity(this WebApplicationBuilder builder)
        {
            builder.Services
            .AddMailSender(builder.Configuration)
            .AddIdentityServices(builder.Configuration)
            .AddEntityFrameworkStores<AppDbContext>();

            return builder;
        }

        public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            return builder;
        }
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddServices()
                .AddSqlRepositories();
            return builder;
        }
    }
}
