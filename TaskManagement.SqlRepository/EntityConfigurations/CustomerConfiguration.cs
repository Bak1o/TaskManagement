using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Models.Enums;

namespace TaskManagement.SqlRepository.EntityConfigurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(a => a.Id);

            builder
                .Property(a => a.UserName)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property (a => a.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property(a => a.Password)
                .IsRequired()
                .HasMaxLength(16);
            builder
                .Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            builder
                .Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(50);
            //builder
            //    .Property(a => a.Role)
            //    .IsRequired(false)
            //    .HasConversion(
            //    v => v.ToString(),
            //    v => (Role)Enum.Parse(typeof(Role), v));

            builder
               .Property(a => a.Role)
               .IsRequired(false) // Role is optional
               .HasConversion(
                v => v != null ? v.ToString() : null, // Enum -> String (handles null)
                v => v != null ? (Role)Enum.Parse(typeof(Role), v) : default // String -> Enum (handles null safely)
               );

            builder
                .Property(a => a.Status)
                .IsRequired()
                .HasConversion(
                v => v.ToString(),
                v => (UserStatus)Enum.Parse(typeof(UserStatus), v));
            //builder
            //    .Property(a => a.CreatedProjects);
            //builder
            //    .Property(a => a.CreatedTasks);

            builder.HasIndex(c => c.Status);

            //builder
            //    .HasOne(a => a.Task)
            //    .WithMany(t => t.AssignedCustomers)
            //    .HasForeignKey(a => a.TaskId)
            //    .OnDelete(DeleteBehavior.SetNull);
            // Relationship: Customer is assigned to a Project (One Project -> Many Customers)
            builder.HasOne(a => a.Project)
                .WithMany()
                .HasForeignKey(a => a.ProjectId)
                .OnDelete(DeleteBehavior.SetNull); // If Project is deleted, set Customer.ProjectId to NULL

            // Relationship: Customer can create multiple Tasks
            builder.HasMany(a => a.CreatedTasks)
                .WithOne(t => t.CreatedByCustomer)
                .HasForeignKey(t => t.CreatedByCustomerId)
                .OnDelete(DeleteBehavior.Cascade); // If Customer is deleted, delete their created tasks

            // Relationship: Customer can create multiple Projects
            builder.HasMany(a => a.CreatedProjects)
                .WithOne(p => p.CreatedByCustomer)
                .HasForeignKey(p => p.CreatedByCustomerId)
                .OnDelete(DeleteBehavior.Cascade); // If Customer is deleted,delete their created projects


        }
    }
}
