using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Identity.Models;
using TaskManagement.SqlRepository.EntityConfigurations;


namespace TaskManagement.SqlRepository.Database
{
    public sealed class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        
        public DbSet <Project> Projects { get; set; }
        public DbSet <DomainTask> DomainTasks { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<TaskUser> TaskUsers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new ProjectConfiguration()); 
            modelBuilder.ApplyConfiguration(new DomainTaskConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
            modelBuilder.ApplyConfiguration(new TaskUserConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());

        }
    }
}
