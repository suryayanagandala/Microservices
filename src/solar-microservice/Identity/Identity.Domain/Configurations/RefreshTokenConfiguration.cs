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
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokens>
    {
        public void Configure(EntityTypeBuilder<RefreshTokens> builder)
        {
            builder.ToTable("RefreshTokens");
            builder.HasKey(x => x.TokenId);

            builder.Property(x => x.RefreshToken).IsRequired();

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.Property(x => x.ExpiryDate).IsRequired();

            builder.HasOne(x=>x.Users).WithMany(x=>x.RefreshTokens).HasForeignKey(x=>x.UserId);
        }
    }
}
