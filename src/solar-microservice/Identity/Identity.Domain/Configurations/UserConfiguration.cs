using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.CoreModels.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.UserId);

            builder.Property(x=>x.UserName).HasMaxLength(100).IsRequired();

            builder.HasIndex(x=>x.Email).IsUnique();

            builder.Property(x=>x.Email).HasMaxLength(255).IsRequired();

            builder.Property(x=>x.PasswordHash).IsRequired();

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.Property(x => x.PhoneNumber).IsRequired();

            builder.Property(x=>x.PhoneNumber).HasMaxLength(10);

            builder.Property(x=>x.Address).IsRequired();

            builder.HasOne(x=>x.Role).WithMany(x=>x.Users).HasForeignKey(x=>x.RoleId);

            builder.HasMany(x => x.RefreshTokens).WithOne(x => x.Users).HasForeignKey(x => x.UserId);

        }
    }
}
