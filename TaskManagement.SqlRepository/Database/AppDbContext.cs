using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.SqlRepository.EntityConfigurations;

namespace TaskManagement.SqlRepository.Database
{
    public sealed class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers {  get; set; }
        public DbSet <Project> Projects { get; set; }
        public DbSet <DomainTask> DomainTasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration()); 
            modelBuilder.ApplyConfiguration(new DomainTaskConfiguration());
            
        }
    }
}
