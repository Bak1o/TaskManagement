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
    public class DomainTaskConfiguration : IEntityTypeConfiguration<DomainTask>
    {
        public void Configure(EntityTypeBuilder<DomainTask> builder)
        {
            builder.ToTable("DomainTasks");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(4000);
            builder
                .Property(t => t.ProjectId)
                .IsRequired();
            builder
                .Property(t => t.CreatedByCustomerId)
                .IsRequired();
            builder
                .Property(t => t.Status)
                .IsRequired()
                .HasConversion(
                v => v.ToString(),
                v => (Status)Enum.Parse(typeof(Status), v));
            builder
              .Property(t => t.Priority)
              .IsRequired()
              .HasConversion(
              v => v.ToString(),
              v => (Priority)Enum.Parse(typeof(Priority), v));
            builder
                .Property(t => t.StartDate)
                .IsRequired();
            builder
                .Property(t => t.DeadLine)
                .IsRequired();
            //builder
            //    .Property(t => t.AssignedCustomers);


            builder.HasIndex(t => t.ProjectId).IncludeProperties(t => new { t.Status, t.Priority });
          
            // Relationship: Task is created by one Customer
            builder
                .HasOne(t => t.CreatedByCustomer)
                .WithMany(c => c.CreatedTasks)
                .HasForeignKey(t => t.CreatedByCustomerId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
        .HasMany(t => t.AssignedCustomers)
        .WithMany(c => c.AssignedTasks)
        .UsingEntity<Dictionary<string, object>>(
            "TaskCustomer",
            j => j.HasOne<Customer>().WithMany().HasForeignKey("CustomerId"),
            j => j.HasOne<DomainTask>().WithMany().HasForeignKey("TaskId")
        );

           

            // Relationship: Task belongs to one Project
            builder
                .HasOne(t => t.Project) // Each Task is linked to one Project
                .WithMany(p => p.Tasks) // One Project can have multiple Tasks
                .HasForeignKey(t => t.ProjectId) // Foreign key in Task table
                .OnDelete(DeleteBehavior.Cascade); // If Project is deleted, all related tasks are deleted

        }
    }
}
