using TaskManagement.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Add this using directive
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagement.Api.Middlewares;

using TaskManagement.Api;
using Serilog;
using TaskManagement.SqlRepository.Database;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Identity.Services.Implementations;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using TaskManagement.Domain.Models.Enums;
using Microsoft.AspNetCore.HttpOverrides;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
     .AddJsonOptions(options =>
     {
         options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
     }); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder
    .AddSwaggerDocumentation()
    .AddJwtAuthentication()
    .AddAuthorizationPolicies()
    .AddApplicationServices()
    .AddReloadableAppSettings()
    .AddSerilog()
    .AddDatabase()
    .AddIdentity();


builder.Services.Configure<ForwardedHeadersOptions>(options =>
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
                                | ForwardedHeaders.XForwardedProto
                                | ForwardedHeaders.XForwardedHost);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseForwardedHeaders();

app.UseErrorHandlingMiddleware();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
