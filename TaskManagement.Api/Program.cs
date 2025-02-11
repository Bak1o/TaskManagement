using TaskManagement.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Add this using directive
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagement.Api.Middlewares;
using TaskManagement.FileRepository.Models;
using TaskManagement.Api;
using Serilog;
using TaskManagement.SqlRepository.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddApplicationServices();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration.GetValue<string>("Authentication:SecretKey"))),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<FileStorageOptions>(builder.Configuration.GetSection("FileStorageOptions"));
//builder.Services.Configure<DatabaseOptions>(DatabaseOptions.SystemDatabaseSectionName,
//    builder.Configuration.GetSection($"{DatabaseOptions.SectionName}: {DatabaseOptions.SystemDatabaseSectionName}"));
//builder.Services.Configure<DatabaseOptions>(DatabaseOptions.BusinessDatabaseSectionName,
//    builder.Configuration.GetSection($"{DatabaseOptions.SectionName} : {DatabaseOptions.BusinessDatabaseSectionName}"));

//var logger = new LoggerConfiguration()
//    .WriteTo.Console()
//    .WriteTo.File("logs/log-")
//    .CreateLogger();
//builder.Logging.ClearProviders();
//builder.Logging.AddSerilog(logger);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseErrorHandlingMiddleware();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
