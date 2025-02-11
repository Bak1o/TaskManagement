using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagement.Domain.Exceptions;

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
                case UnauthorizedAccessException:
                    problemDetails.Title = "Unauthorized access.";
                    problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Type = nameof(UnauthorizedAccessException);
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
