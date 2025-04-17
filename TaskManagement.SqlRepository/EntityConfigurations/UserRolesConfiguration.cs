using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.SqlRepository.EntityConfigurations
{
    public class UserRolesConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole { Id = "E02E3796-05AB-4AB2-99E8-DF9F60C384A6".ToString(), Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "A1218D50-BEB9-4605-85A5-7E803C76CC20".ToString(), Name = "Manager", NormalizedName = "MANAGER" },
                new IdentityRole { Id = "297EAACE-F9C9-4ECC-9605-6DAEC6EA701A".ToString(), Name = "Member", NormalizedName = "MEMBER" }
            );
        }
    }
}
