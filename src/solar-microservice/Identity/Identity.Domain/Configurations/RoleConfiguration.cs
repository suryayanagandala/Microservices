using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.CoreModels.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Roles>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(x => x.RoleId);

            builder.Property(x=>x.RoleName).HasMaxLength(50).IsRequired();

            builder.HasIndex(x => x.RoleName).IsUnique();
        }
    }
}
