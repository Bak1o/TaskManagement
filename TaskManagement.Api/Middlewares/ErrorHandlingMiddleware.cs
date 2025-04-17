using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Identity.Exceptions;
using TaskManagement.Service.Exceptions;
using UnauthorizedAccessException = TaskManagement.Identity.Exceptions.UnauthorizedAccessException;

namespace TaskManagement.Api.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occured");
                await HandleExceptionAsync(httpContext, ex);
            }
           
        }

        public static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // define default status code and response
            
            var problemDetails = new ProblemDetails
            {
                Title = "An unexpected error occurred.",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = exception.Message, // Optionally remove this in production
                Instance = context.TraceIdentifier,
            };
            switch(exception)
            {
                case System.UnauthorizedAccessException:
                    problemDetails.Title = "Unauthorized access.";
                    problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Type = nameof(UnauthorizedAccessException);
                    break;

                case UnauthorizedAccessException:
                    problemDetails.Title = exception.Message;
                    problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Type = nameof(UnauthorizedAccessException);
                    break;

                case AuthenticationException:
                    problemDetails.Title = exception.Message;
                    problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Type = nameof(AuthenticationException);
                    break;

                case ChangePasswordException ex:
                    problemDetails.Title = exception.Message;
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Type = nameof(ChangePasswordException);
                    problemDetails.Extensions = ex.Errors?.ToDictionary(x => x.Code, object? (x) => x.Description) ??
                                                problemDetails.Extensions;
                    break;

                case IdentityException ex:
                    problemDetails.Title = exception.Message;
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Type = nameof(IdentityException);
                    problemDetails.Extensions = ex.Errors?.ToDictionary(x => x.Code, object? (x) => x.Description) ??
                                                problemDetails.Extensions;
                    break;
                
                case UserValidationException ex:
                    problemDetails.Title = exception.Message;
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Type = nameof(UserValidationException);
                    
                    break;

                case NotFoundException ex:
                    problemDetails.Title = exception.Message;
                    problemDetails.Status = (int)HttpStatusCode.NotFound;
                    problemDetails.Type = nameof(NotFoundException);

                    break;

                case ArgumentNullException:
                    problemDetails.Title = "Invalid request data.";
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Type = nameof(ArgumentException);
                    break;
                case ArgumentException:
                    problemDetails.Title = "Invalid request data.";
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Type = nameof(ArgumentException);
                    break;

                case KeyNotFoundException:
                    problemDetails.Title = "Resource not found.";
                    problemDetails.Status = (int)HttpStatusCode.NotFound;
                    problemDetails.Type = nameof(KeyNotFoundException);
                    break;

                case ObjectNotFoundException:
                    problemDetails.Title = "Object not found";
                    problemDetails.Status = (int)HttpStatusCode.NotFound;
                    problemDetails.Type = nameof(ObjectNotFoundException);
                    break;

                case ValidationException:
                    problemDetails.Title = "Validation violation";
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Type = nameof(ValidationException);
                    break;

                case DomainException:
                    problemDetails.Title = "Domain specific logic violation";
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Type = nameof(DomainException);
                    break;

                case ConfigurationException:
                    problemDetails.Title = "Configuration violation";
                    problemDetails.Status= (int)HttpStatusCode.BadRequest;
                    problemDetails.Type = nameof(ConfigurationException);
                    break;

               
                


            }
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problemDetails.Status.Value;


            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var result = JsonSerializer.Serialize(problemDetails, options);
            return context.Response.WriteAsync(result);
        }
    }

    

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
