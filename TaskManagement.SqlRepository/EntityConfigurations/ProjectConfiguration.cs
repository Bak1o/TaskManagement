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
    public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder
                .Property(x => x.Description)
                .HasMaxLength(4000)
                .IsRequired();
            builder
                .Property(x => x.StartDate);
            builder
                .Property (x => x.EndDate);
            builder
                .Property(x => x.Status)
                .IsRequired()
                .HasConversion(
                v => v.ToString(),
                v => (Status)Enum.Parse(typeof(Status), v)
                );
            builder
                .Property(x => x.CreatedByUserId)
                .IsRequired();
            //builder
            //    .Property(x => x.Tasks);

            builder.HasIndex(x => x.Status);
            builder
                .HasOne(c => c.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting customer if projects exist
            //builder
            //    .HasMany(p => p.Tasks) // One Project has many Tasks
            //    .WithOne(t => t.Project) // Each Task belongs to one Project
            //    .HasForeignKey(t => t.ProjectId) // Foreign key in DomainTask table
            //    .OnDelete(DeleteBehavior.Cascade); // If Project is deleted, Tasks are deleted

        }
    }
}
