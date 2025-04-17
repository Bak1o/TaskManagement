using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;


namespace TaskManagement.SqlRepository.EntityConfigurations
{
    public sealed class TaskUserConfiguration : IEntityTypeConfiguration<TaskUser>
    {
        public void Configure(EntityTypeBuilder<TaskUser> builder)
        {
           builder.HasKey(tu => new { tu.DomainTaskId, tu.ApplicationUserId });

            builder
                .HasOne(tu => tu.DomainTask)
                .WithMany(t => t.AssignedUsers)
                .HasForeignKey(tu => tu.DomainTaskId);

            builder
                .HasOne(tu => tu.ApplicationUser)
                .WithMany()
                .HasForeignKey(tu => tu.ApplicationUserId);
        }
    }
}
